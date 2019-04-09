using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace BattleCity
{
    class Level
    {
        public string Name { get; set; } = "NoNameLevel";
        public TypeOfBlock[,] FieldInfo;
        public (int,int)[] NPCsInfo;
        public (int, int) PlayerInfo;
    }

    class MultiplayerLevel : Level
    {
        public (int, int) SecondPlayerInfo;
    }
}
