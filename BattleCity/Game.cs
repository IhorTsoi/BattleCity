using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BattleCity
{
    class Game
    {
        #region Fields & Properties

        private Timer gameTimer;

        public Field Field { get; set; }

        public PlayerModel Player { get; set; }
        public List<NPCModel> NPCs;
        public List<Bullet> Bullets;

        public bool gameOver = false;
        private bool _npcTime = false;

        public string LvlName;

        #endregion

        #region Constructors

        public Game() { }

        public Game(Level levelInfo)
        {
            this.LvlName = levelInfo.Name;

            this.Field = new Field ( mapInfo: levelInfo.FieldInfo );

            this.Player = new PlayerModel(position: levelInfo.PlayerInfo, field: this.Field, game: this);
            this.NPCs = new List<NPCModel> ();
            foreach ((int, int) position in levelInfo.NPCsInfo )
            {
                NPCs.Add(new NPCModel(position: position, field: this.Field, player: this.Player, game: this));
            }
            this.Bullets = new List<Bullet>();
        }

        #endregion

        public void StartGame()
        {
            Console.Clear();
            StartTimer();
            this.Field._firstRender();
        }

        #region Start, Stop, Render

        public void StartTimer()
        {
            gameTimer = new Timer(interval: 180);
            gameTimer.Elapsed += Render;
            gameTimer.AutoReset = true;
            gameTimer.Enabled = true;
        }

        public void StopTimer()
        {
            gameTimer.Stop();
            gameTimer.Dispose();
        }

        private void Render(object sender, ElapsedEventArgs e)
        {
            //Console.Clear();
                
            // moving
            this.Player.MoveHero();
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
                if (this.Field.map[bullet.Position.Item1, bullet.Position.Item2].Type != TypeOfBlock.EmptyCell)
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
            this.Player.Shoot();

            _npcTime = !_npcTime;




            // fin
            if (this.NPCs.Count == 0)
            {
                this.StopTimer();
                this.gameOver = true;
            }


            if (!gameOver)
            {
                this.Field.Render();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Clear();
                Console.WriteLine(new string('\n', 7) + (
                                                            (this.NPCs.Count == 0) ?
                                                                "\tYOU WON! PRESS ANY BUTTON!" :
                                                                "\tGAME OVER. PRESS ANY BUTTON.")
                                                        );
            }
        }

        #endregion
    }
}
