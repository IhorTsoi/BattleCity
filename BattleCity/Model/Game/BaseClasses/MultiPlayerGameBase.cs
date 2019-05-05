using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace BattleCity.Model.Game
{
    abstract class MultiPlayerGameBase : GameBase
    {
        protected Socket CLIENT;
        protected PlayerModel Opponent { get; set; }
        protected byte[] Buffer_movement = new byte[1];
        protected byte[] Buffer_shooting = new byte[1];
        protected byte[] Buffer_presence = new byte[1];
        protected bool CloseMessage = false;

        // Constructor:
        public MultiPlayerGameBase(MultiplayerLevel multiplayerLevel)
        {
            LvlName = multiplayerLevel.Name;

            Field = new Field(mapInfo: multiplayerLevel.FieldInfo);
            Bullets = new List<Bullet>();

            ConnectToOpponent();
        }

        // Overriden methods:
        public override void Quit() => CloseMessage = true;

        // Abstract methods:
        protected abstract void ConnectToOpponent();

        // Protected methods:
        protected void Communicate(SocketCommunication mode, object sender = null)
        {
            byte message;

            switch (mode)
            {
                case SocketCommunication.SendMove:
                    message = 4;
                    if ((Directions?)sender != null)
                    {
                        message = (byte)(Directions?)sender;
                    }
                    CLIENT.Send(new byte[] { message });
                    break;
                case SocketCommunication.ReceiveMove:
                    CLIENT.Receive(Buffer_movement);
                    ProcessInfo(Buffer_movement[0]);
                    break;
                case SocketCommunication.SendShoot:
                    message = (byte)((bool)sender ? 1 : 0);
                    CLIENT.Send(new byte[] { message });
                    break;
                case SocketCommunication.ReceiveShoot:
                    CLIENT.Receive(Buffer_shooting);
                    ProcessInfo(Buffer_shooting[0], shootMode: true);
                    break;
                default:
                    throw new Exception("Not implemented SocketCommunication enum.");
            }
        }
        protected void ProcessInfo(byte v, bool shootMode = false)
        {
            if ( !shootMode )
            {
                if (v == 4) // means null-signal
                {
                    Opponent.NextStep = null;
                }
                else // means direction
                {
                    Opponent.NextStep = (Directions)v;
                }
            }
            else
            {
                Opponent.NextShoot = (v == 0) ?
                                                false :
                                                    true;
            }
        }
    }
}