using System.Collections.Generic;

namespace BattleCity
{
    enum SocketCommunication
    {
        SendMove,
        RecieveMove,
        SendShoot,
        RecieveShoot
    }

    interface IBasicGame
    {
        // properties:
        PlayerModel Player { get; set; }
        List<NPCModel> NPCs { get; set; }
        List<Bullet> Bullets { get; set; }
        string LvlName { get; set; }
        bool _gameOver { get; set; }
        bool _won { get; set; }

        // methods:
        void StartGame();
        void Quit();
    }
}