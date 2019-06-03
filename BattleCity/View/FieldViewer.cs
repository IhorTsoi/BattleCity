using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity.Views
{
    abstract class FieldViewer
    {
        protected Queue<BlockState> blocksToRender = new Queue<BlockState>();
        protected static FieldViewer instance;

        public void Update((int Y, int X) position, TypeOfBlock type, Directions direction = Directions.Left)
        {
            blocksToRender.Enqueue(new BlockState(position, type, direction));
        }

        public abstract void Render();

        public static FieldViewer GetInstance() => instance;
    }
}
