using System;
using System.Collections.Generic;
using System.Text;

namespace Guild
{
    class Player
    {
        private string rank = "Trial";

        private string description = "n/a";

        public Player(string name, string classs)
        {
            Name = name;
            Class = classs;

        }
        public string Name { get; set; }

        public string Class { get; set; }

        public string Rank { get { return rank; } set { rank = value; } }

        public string Description { get { return description; } set { description = value; } }

        public override string ToString()
        {
            StringBuilder myStringToReturn = new StringBuilder();
            myStringToReturn.AppendLine($"Player {this.Name}: {this.Class}");
            myStringToReturn.AppendLine($"Rank: {this.Rank}");
            myStringToReturn.AppendLine($"Description: {this.Description}");
            return myStringToReturn.ToString().TrimEnd();

        }
    }
}
