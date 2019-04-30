using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCity
{
    class User
    {
        #region Fields & Properties

        public string Name { get; set; } = "User";
        public List<string> Levels { get; set; } = new List<string>();
       
        #endregion

        #region Constructors

        public User() { }

        public User(string name, List<string> levels)
        {
            this.Name = name;
            this.Levels = levels;
        }

        #endregion
    }
}
