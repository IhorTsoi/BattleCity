using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using BattleCity.Model.Components;

namespace BattleCity.Model.Game
{
    class ClientGame : MultiPlayerGameBase
    {
        // Thread for mainloop
        private Thread newThread;

        // Constructors:
        public ClientGame(MultiplayerLevel multiplayerLevel) : base(multiplayerLevel)
        {
            Player = new PlayerModel(position: multiplayerLevel.SecondPlayerInfo, field: Field, game: this);
            Player.DieEvent += () =>
            {
                GameState.Lose();
                CloseMessage = true;
            };
            //
            Opponent = new PlayerModel(position: multiplayerLevel.PlayerInfo, field: Field, game: this);
            Opponent.DieEvent += () =>
            {
                GameState.Win();
                CloseMessage = true;
            };
            //
            NPCs = new List<NPCModel>();
            foreach ((int, int) position in multiplayerLevel.NPCsInfo)
            {
                NPCs.Add(
                    new NPCModelMP( position,
                        Field,
                        player: Opponent,
                        game: this,
                        serverPlayer: Opponent,
                        clientPlayer: Player));
            }
        }


        // Overriden methods:
        protected override void StartLoop() {
            newThread = new Thread(MainLoop);
            newThread.Start();
        }
        protected override void ConnectToOpponent()
        {
            CLIENT = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            CLIENT.Connect("127.0.0.1", 8080);
        }
        private void StopTheGame()
        {
            CLIENT.Close();
            CLIENT.Dispose();
            View.PrintGameOver(Won);
            GameOver = true;
        }


        // Private methods:
        private bool CheckOpponentOnline()
        {
            // 0 if disconnected, 1 if connected
            CLIENT.Receive(Buffer_presence);
            
            byte answer = (byte)(CloseMessage ? 0 : 1);
            CLIENT.Send(new byte[] { answer });
            return Buffer_presence[0] == 1 && answer == 1;
        }

        private void MainLoop()
        {
            while (!GameOver)
            {
                // IF THE OPPONENT IS NOT ONLINE
                if (!CheckOpponentOnline())
                {
                    StopTheGame();
                    break;
                }


                // MOVING
                Communicate(SocketCommunication.ReceiveMove);
                Opponent.MoveHero();
                
                Communicate(SocketCommunication.SendMove, 
                    Player.MoveHero());

                if (_npcTime)
                {
                    InvokeNPCs();
                }

                // INVOKING BULLETS
                InvokeBullets();

                // SHOOTING
                if (_npcTime)
                {
                    InvokeNPCs(shoot: true); 
                }

                Communicate(SocketCommunication.ReceiveShoot);
                Opponent.Shoot();

                Communicate(SocketCommunication.SendShoot, 
                    Player.Shoot());

                
                // RENDERING THE MAP
                _npcTime = !_npcTime;
                Field.RenderCommon();
            }
        }
    }
}
