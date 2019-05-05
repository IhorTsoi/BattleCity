using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    class User
    {
        // Properties :

        public string Name { get; set; } = "User";
        public List<string> Levels { get; set; } = new List<string>();
       

        // Constructors :

        public User(string name, List<string> levels)
        {
            Name = name;
            Levels = levels;
        }
    }
}
