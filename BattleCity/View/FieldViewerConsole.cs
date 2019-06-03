using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity.Views
{
    class FieldViewerConsole : FieldViewer
    {
        Dictionary<TypeOfBlock, BlockViewConsole> blocksView;

        //
        // FOR TESTING:
        //
        /*
        Dictionary<TypeOfBlock, BlockViewConsole> blocksView = new Dictionary<TypeOfBlock, BlockViewConsole>
        {
            [TypeOfBlock.Player] = new BlockViewConsole(
                symbols: new char[] { '◄', '▲', '►', '▼' },
                backGroundColor: ConsoleColor.DarkGray,
                foreGroundColor: ConsoleColor.Black),
            //
            [TypeOfBlock.NPC] = new BlockViewConsole(
                symbols: new char[] { '←', '↑', '→', '↓' },
                backGroundColor: ConsoleColor.DarkGray,
                foreGroundColor: ConsoleColor.Black),
            //
            [TypeOfBlock.Bullet] = new BlockViewConsole(
                symbols: new char[] { 'ᴏ' },
                backGroundColor: ConsoleColor.DarkGray,
                foreGroundColor: ConsoleColor.DarkRed),
            //
            [TypeOfBlock.EmptyCell] = new BlockViewConsole(
                symbols: new char[] { ' ' },
                backGroundColor: ConsoleColor.DarkGray,
                foreGroundColor: ConsoleColor.DarkGray),
            //
            [TypeOfBlock.BrickWall] = new BlockViewConsole(
                symbols: new char[] { '□' },
                backGroundColor: ConsoleColor.DarkBlue,
                foreGroundColor: ConsoleColor.DarkGray),
            //
            [TypeOfBlock.Wall] = new BlockViewConsole(
                symbols: new char[] { '#' },
                backGroundColor: ConsoleColor.Black,
                foreGroundColor: ConsoleColor.Black),
        };
        */

        public static void Initialize(Dictionary<TypeOfBlock, BlockViewConsole> viewConfig)
        {
            instance = new FieldViewerConsole(viewConfig);
        }

        protected FieldViewerConsole(Dictionary<TypeOfBlock, BlockViewConsole> viewConfig)
        {
            blocksView = viewConfig;
        }

        public override void Render()
        {
            while(blocksToRender.Count != 0)
            {
                BlockState BLOCK = blocksToRender.Dequeue();

                // Set the position:
                Console.SetCursorPosition(top: BLOCK.Position.Y, left: BLOCK.Position.X);

                // Get the drawing context:
                Console.BackgroundColor = blocksView[BLOCK.Type].BackGroundColor;
                Console.ForegroundColor = blocksView[BLOCK.Type].ForeGroundColor;

                // Get the symbol:
                char symbol = blocksView[BLOCK.Type].Symbols[(int)BLOCK.Direction];

                Console.Write(symbol);
            }
        }
    }
}
