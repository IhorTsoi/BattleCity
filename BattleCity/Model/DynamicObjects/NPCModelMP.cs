using System;
using System.Collections.Generic;
using BattleCity.Model.Game;

namespace BattleCity
{
    class NPCModelMP : NPCModel
    {
        // Refferences to players:
        private readonly PlayerModel ServerPlayer;
        private readonly PlayerModel ClientPlayer;

        // Constructor:
        public NPCModelMP((int Y, int X) position, Field field, PlayerModel player, IGame game, PlayerModel serverPlayer, PlayerModel clientPlayer)
            : base(position, field, player, game)
        {
            ServerPlayer = serverPlayer;
            ClientPlayer = clientPlayer;
        }

        // Overriden methods:
        public override void AIMove()
        {
            //
            // Method ChoosePlayer chooses the closest player.
            //
            ChoosePlayer();
            //
            base.AIMove();
        }

        // Private methods:
        private void ChoosePlayer()
        {
            //
            int DistanceToServerPlayer = Math.Min(
                Math.Abs((Position.X - ServerPlayer.Position.X)),
                Math.Abs((Position.Y - ServerPlayer.Position.Y)));
            //
            int DistanceToClientPlayer = Math.Min(
                Math.Abs((Position.X - ClientPlayer.Position.X)),
                Math.Abs((Position.Y - ClientPlayer.Position.Y)));
            //
            Player = (DistanceToServerPlayer > DistanceToClientPlayer) ?
                                                            ClientPlayer :
                                                                ServerPlayer;
        }
    }
}
