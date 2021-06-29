using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guild
{
    class Guild
    {
        private List<Player> roaster;

        public Guild(string name , int capacity)
        {
            Name = name;
            Capacity = capacity;
            roaster = new List<Player>();
        }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public int Count { get { return roaster.Count; } }

        public void AddPlayer(Player player)
        {
            if (roaster.Count<Capacity)
            {
                roaster.Add(player);
            }
        }
        
        public bool RemovePlayer(string name)
        {
            Player player = roaster.Where(p => p.Name == name).FirstOrDefault();
            if (player!=null)
            {
                roaster.Remove(player);
                return true;
            }
            return false;
        }

        public Player PromotePlayer(string name)
        {
            Player player = roaster.FirstOrDefault(p => p.Name == name && p.Class != "Member");
            player.Rank = "Member";
            return player;
        }

        public Player DemotePlayer(string name)
        {
            Player player = roaster.FirstOrDefault(p => p.Name == name && p.Class != "Trial");
            player.Rank = "Trial";
            return player;
        }

        public Player[] KickPlayersByClass(string classs)
        {

            List<Player> myListTemp = new List<Player>();
            foreach (var player in this.roaster)
            {
                if (player.Class == classs)
                {
                    myListTemp.Add(player);
                }
            }
            Player[] myArrayToReturn = myListTemp.ToArray();

            this.roaster = this.roaster.Where(x => x.Class != classs).ToList();

            return myArrayToReturn;
        }

        public string Report()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"Players in the guild: {this.Name}");
            foreach (var player in roaster)
            {
                result.AppendLine($"Player {player.Name}: {player.Class}");
                result.AppendLine($"Rank: {player.Rank}");
                result.AppendLine($"Description: {player.Description}");
            }
            return result.ToString().Trim();
        }
    }
}
