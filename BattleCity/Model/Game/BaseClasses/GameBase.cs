using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleCity.Model.Components;

namespace BattleCity.Model.Game
{
    abstract class GameBase : IGame
    {
        // dynamic objects:
        public PlayerModel Player { get;set; }
        public List<NPCModel> NPCs { get;set; }
        public List<Bullet> Bullets { get;set; }
        public Field Field { get; set; }

        // game data:
        public string LvlName { get;set; }
        public bool GameOver { get; set; } = false;
        public bool Won { get => GameState.GetState(); }

        // protected properties:
        protected bool _npcTime { get; set; } = true;
        protected GameState GameState;

        // abstract methods:
        public abstract void Quit();
        protected abstract void StartLoop();

        // protected methods:
        public void StartGame()
        {
            Console.Clear();
            Field.RenderCommon(firstRender: true);
            StartLoop();
        }
        protected void InvokeNPCs(bool shoot = false)
        {
            foreach (NPCModel npc in NPCs.ToList())
            {
                if (shoot)
                {
                    npc.AIShoot();
                }
                else
                {
                    npc.AIMove();
                }
            }
        }
        protected void InvokeBullets()
        {
            foreach (Bullet bullet in Bullets.ToList())
            {
                if (Field[bullet.Position.Y, bullet.Position.X].Type != TypeOfBlock.EmptyCell)
                {
                    bullet.MoveBullet();
                }
            }
        }
    }
}