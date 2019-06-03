using System.Collections.Generic;

namespace BattleCity.Model.Game
{
    enum SocketCommunication
    {
        SendMove,
        ReceiveMove,
        SendShoot,
        ReceiveShoot
    }

    interface IGame
    {
        // properties:
        PlayerModel Player { get; set; }
        List<NPCModel> NPCs { get; set; }
        List<Bullet> Bullets { get; set; }
        Field Field { get; set; }
        string LvlName { get; set; }
        bool GameOver { get; set; }
        bool Won { get; }

        // methods:
        void StartGame();
        void Quit();
    }
}