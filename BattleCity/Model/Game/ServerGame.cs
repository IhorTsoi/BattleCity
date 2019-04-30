using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Net;
using System.Net.Sockets;
using BattleCity.Model.Game.Components;

namespace BattleCity.Model.Game
{
    class ServerGame : MultiPlayerGameBase
    {
        Socket SERVER;
        private GameTimer GameTimer;

        // Constructors:
        public ServerGame(MultiplayerLevel multiplayerLevel) : base(multiplayerLevel)
        {
            Player = new PlayerModel(position: multiplayerLevel.PlayerInfo, field: Field, game: this);
            Player.DieEvent += () =>
                                    {
                                        GameState.Lose();
                                        CloseMessage = true;
                                    };
            //
            Opponent = new PlayerModel(position: multiplayerLevel.SecondPlayerInfo, field: Field, game: this);
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
                        player: Player,
                        game: this,
                        serverPlayer: Player,
                        clientPlayer: Opponent));
            }
            //
            GameTimer = new GameTimer(loop_function: MainLoop);
        }

        // Overriden methods:
        protected override void StartLoop()
        {
            GameTimer.StartTimer();
        }
        protected override void ConnectToOpponent()
        {
            SERVER = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SERVER.Bind(new IPEndPoint(IPAddress.Any, 8080));
            SERVER.Listen(backlog: 1);
            CLIENT = SERVER.Accept();
        }
        private void StopTheGame()
        {
            GameTimer.StopTimer();
            GameOver = true;
            // SOCKETS DISPOSE
            CLIENT.Close();
            CLIENT.Dispose();
            SERVER.Close();
            SERVER.Dispose();
            // SIMPLE OUTPUT
            View.PrintGameOver(Won);
        }

        private bool CheckOpponentOnline()
        {
            // 0 if disconnected, 1 if connected
            byte answer = (byte)(CloseMessage ? 0 : 1);
            CLIENT.Send(new byte[] { answer });

            CLIENT.Receive(Buffer_presence);
            return Buffer_presence[0] == 1 && answer == 1;
        }

        private void MainLoop(object sender, ElapsedEventArgs e)
        {
            // IF THE OPPONENT IS NOT ONLINE
            if (!CheckOpponentOnline())
            {
                StopTheGame();
                return;
            }

            // MOVING
            Communicate(SocketCommunication.SendMove, 
                this.Player.MoveHero());
            
            Communicate(SocketCommunication.ReceiveMove);
            this.Opponent.MoveHero();
            
            if (_npcTime)
            {
                InvokeNPCs(shoot: false);
            }

            // INVOKING BULLETS
            InvokeBullets();

            // SHOOTING
            if (_npcTime)
            {
                InvokeNPCs(shoot: true);
            }

            Communicate(SocketCommunication.SendShoot, 
                this.Player.Shoot());
      
            Communicate(SocketCommunication.ReceiveShoot);
            this.Opponent.Shoot();


            // RENDERING THE MAP
            _npcTime = !_npcTime;
            this.Field.RenderCommon();
        }
    }
}