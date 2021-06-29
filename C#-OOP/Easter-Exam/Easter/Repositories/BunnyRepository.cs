using Easter.Models.Bunnies;
using Easter.Models.Bunnies.Contracts;
using Easter.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easter.Repositories
{
    public class BunnyRepository : IRepository<Bunny>
    {
        private List<Bunny> bunnies;

        public BunnyRepository()
        {
            this.bunnies = new List<Bunny>();
        }

        public IReadOnlyCollection<Bunny> Models => this.bunnies.AsReadOnly();

        public void Add(Bunny model)
        {
            bunnies.Add(model);
        }

        public Bunny FindByName(string name)
        {
            return bunnies.FirstOrDefault(x => x.Name == name);
        }

        public bool Remove(Bunny model)
        {
            if (bunnies.Contains(model))
            {
                bunnies.Remove(model);
                return true;
            }
            return false;
        }
    }
}
