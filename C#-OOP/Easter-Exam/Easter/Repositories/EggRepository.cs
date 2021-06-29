using Easter.Models.Eggs;
using Easter.Models.Eggs.Contracts;
using Easter.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easter.Repositories
{
    public class EggRepository : IRepository<Egg>
    {
        private List<Egg> eggs;

        public EggRepository()
        {
            this.eggs = new List<Egg>();
        }

        public IReadOnlyCollection<Egg> Models => this.eggs.AsReadOnly();

        public void Add(Egg model)
        {
            eggs.Add(model);
        }

        public Egg FindByName(string name)
        {
            return eggs.FirstOrDefault(x => x.Name == name);
        }

        public bool Remove(Egg model)
        {
            if (eggs.Contains(model))
            {
                eggs.Remove(model);
                return true;
            }
            return false;
        }
    }
}
