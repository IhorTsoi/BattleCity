using BattleCity.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using BattleCity.Model.Game.Components;

namespace BattleCity.Model.Game
{
    class OnePlayerGame : GameBase
    {
        private GameTimer GameTimer;

        // Constructors:
        public OnePlayerGame(Level levelInfo)
        {
            this.LvlName = levelInfo.Name;
            //
            this.Field = new Field(mapInfo: levelInfo.FieldInfo);
            //
            this.Player = new PlayerModel(position: levelInfo.PlayerInfo, field: this.Field, game: this);
            this.Player.DieEvent += () => GameState.Lose();
            //
            this.NPCs = new List<NPCModel>();
            foreach ((int, int) position in levelInfo.NPCsInfo)
            {
                NPCs.Add(new NPCModel(position: position, field: this.Field, player: this.Player, game: this));
            }
            //
            this.Bullets = new List<Bullet>();
            //
            GameTimer = new GameTimer(loop_function: MainLoop);
        }

        // Overriden methods:
        protected override void StartLoop()
        {
            GameTimer.StartTimer();
        }
        public override void Quit()
        {
            GameTimer.StopTimer();
            GameOver = true;
            View.PrintGameOver(Won);
        }

        private void MainLoop(object sender, ElapsedEventArgs e)
        {                
            // MOVING
            this.Player.MoveHero();
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
            this.Player.Shoot();

            
            // RENDERING THE MAP
            _npcTime = !_npcTime;
            this.Field.RenderCommon();


            // CHECK IF THE GAME IS OVER
            if ( NPCs.Count == 0 || GameState.Died() )
            {
                GameState.Win();
                Quit();
            }
        }
    }
}
