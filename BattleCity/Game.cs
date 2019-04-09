using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BattleCity
{
    class Game : IBasicGame
    {
        #region Fields & Properties

        protected Timer gameTimer;

        public Field Field { get; set; }

        public PlayerModel Player { get; set; }
        public List<NPCModel> NPCs { get; set; }
        public List<Bullet> Bullets { get; set; }

        public bool _gameOver { get; set; } = false;
        public bool _won { get; set; } = false;
        protected bool _npcTime = false;

        public string LvlName { get; set; }

        #endregion

        #region Constructors

        public Game() { }

        public Game(Level levelInfo)
        {
            this.LvlName = levelInfo.Name;

            this.Field = new Field(mapInfo: levelInfo.FieldInfo);

            this.Player = new PlayerModel(position: levelInfo.PlayerInfo, field: this.Field, game: this);
            this.NPCs = new List<NPCModel>();
            foreach ((int, int) position in levelInfo.NPCsInfo)
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

        public void Quit()
        {
            StopTimer();
            _gameOver = true;
        }

        #region Start, Stop, Render

        public virtual void StartTimer()
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
            this.Player.Shoot();

            _npcTime = !_npcTime;




            // fin
            if (this.NPCs.Count == 0)
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

        #endregion
    }
}
