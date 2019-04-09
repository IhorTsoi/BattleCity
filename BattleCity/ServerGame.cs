using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Net;
using System.Net.Sockets;

namespace BattleCity
{
    class ServerGame : Game
    {       
        Socket SERVER, CLIENT;
        public PlayerModel Opponent { get; set; }
        private byte[] buffer_movement = new byte [1];
        private byte[] buffer_shooting = new byte [1];

        #region Constructors

        public ServerGame() { }

        public ServerGame(MultiplayerLevel levelInfo)
        {
            this.LvlName = levelInfo.Name;

            this.Field = new Field(mapInfo: levelInfo.FieldInfo);

            this.Player = new PlayerModel(position: levelInfo.PlayerInfo, field: this.Field, game: this);
            this.Opponent = new PlayerModel(position: levelInfo.SecondPlayerInfo, field: this.Field, game: this);
            this.NPCs = new List<NPCModel>();
            foreach ((int, int) position in levelInfo.NPCsInfo)
            {
                NPCs.Add(new NPCModel(position: position, field: this.Field, player: this.Player, game: this));
            }
            this.Bullets = new List<Bullet>();

            ConnectToOpponent();
        }

        #endregion


        private void ConnectToOpponent()
        {
            SERVER = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SERVER.Bind(new IPEndPoint(IPAddress.Any, 8080));
            SERVER.Listen(backlog: 1);
            CLIENT = SERVER.Accept();
        }

        private void Communicate(SocketCommunication mode, object sender = null)
        {
            switch (mode)
            {
                case SocketCommunication.SendMove:
                    if ((Directions?)sender != null)
                    {
                        CLIENT.Send(new byte[] { (byte)(Directions)(Directions?)sender });
                    }
                    else
                    {
                        CLIENT.Send(new byte[] { 4 });
                    }

                    break;
                case SocketCommunication.RecieveMove:
                    CLIENT.Receive(buffer_movement);
                    ProcessInfo(buffer_movement[0]);
                    break;
                case SocketCommunication.SendShoot:
                    if ((bool)sender)
                    {
                        CLIENT.Send(new byte[] { 1 });
                    }
                    else
                    {
                        CLIENT.Send(new byte[] { 0 });
                    }

                    break;
                case SocketCommunication.RecieveShoot:
                    CLIENT.Receive(buffer_shooting);
                    ProcessInfo(buffer_shooting[0], shoot: true);

                    break;
                default:
                    break;
            }
        }

        public override void StartTimer()
        {
            gameTimer = new Timer(interval: 180);
            gameTimer.Elapsed += Render;
            gameTimer.AutoReset = true;
            gameTimer.Enabled = true;
        }

        private void ProcessInfo(byte v, bool shoot = false)
        {
            if (!shoot)
            {
                if (v == 4)
                {
                    this.Opponent._nextStep = null;
                }
                else
                {
                    this.Opponent._nextStep = (Directions)v;
                }
            }
            else
            {
                if (v == 0)
                {
                    this.Opponent._nextShot = false;
                }
                else
                {
                    this.Opponent._nextShot = true;
                }
            }
        }

        private void Render(object sender, ElapsedEventArgs e)
        {
            // moving
            Communicate(SocketCommunication.SendMove, this.Player.MoveHero());
            

            Communicate(SocketCommunication.RecieveMove);
            this.Opponent.MoveHero();



            if (_npcTime)
            {
                foreach (NPCModel npc in this.NPCs.ToList())
                {
                    npc.AIMove();
                }
            }
            // invoking bullets

            foreach (Bullet bullet in this.Bullets.ToList())
            {
                if (this.Field.map[bullet.Position.Y, bullet.Position.X].Type != TypeOfBlock.EmptyCell)
                {
                    bullet.MoveBullet();
                }
            }

            // shooting
            if (_npcTime)
            {
                foreach (NPCModel npc in this.NPCs.ToList())
                {
                    npc.AIShoot();
                }
            }

            Communicate(SocketCommunication.SendShoot, this.Player.Shoot());
            

            Communicate(SocketCommunication.RecieveShoot);
            this.Opponent.Shoot();



            _npcTime = !_npcTime;




            // fin
            if (this.NPCs.Count == 0 )
            {
                this.StopTimer();
                this._gameOver = true;
                this._won = true;
            }


            if (!_gameOver)
            {
                this.Field.Render();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Clear();
                Console.WriteLine(new string('\n', 7) + (
                                                            (_won) ?
                                                                "\tYOU WON! PRESS ANY BUTTON!" :
                                                                "\tGAME OVER. PRESS ANY BUTTON.")
                                                        );
            }
        }
    }
}