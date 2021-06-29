using Easter.Core.Contracts;
using Easter.Models.Bunnies;
using Easter.Repositories;
using Easter.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Easter.Models.Dyes;
using Easter.Models.Eggs;
using Easter.Models.Workshops;

namespace Easter.Core
{
    public class Controller : IController
    {
        private BunnyRepository bunnies;
        private EggRepository eggs;
        private int coloredEggs = 0;

        public Controller()
        {
            bunnies = new BunnyRepository();
            eggs = new EggRepository();
        }

        public string AddBunny(string bunnyType, string bunnyName)
        {
            Bunny bunny;
            if (bunnyType == "HappyBunny")
            {
                bunny = new HappyBunny(bunnyName);
            }
            else if(bunnyType == "SleepyBunny")
            {
                bunny = new SleepyBunny(bunnyName);
            }
            else
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidBunnyType);
            }

            bunnies.Add(bunny);
            return $"Successfully added {bunnyType} named {bunnyName}.";
        }

        public string AddDyeToBunny(string bunnyName, int power)
        {
            var bunny = bunnies.Models.FirstOrDefault(x => x.Name == bunnyName);
            if (bunny == null)
            {
                throw new InvalidOperationException("The bunny you want to add a dye to doesn't exist!");
            }
            bunny.AddDye(new Dye(power));
            return $"Successfully added dye with power {power} to bunny {bunnyName}!";
        }

        public string AddEgg(string eggName, int energyRequired)
        {
            var egg = new Egg(eggName, energyRequired);
            eggs.Add(egg);

            return $"Successfully added egg: {eggName}!";
        }

        public string ColorEgg(string eggName)
        {
            if (!bunnies.Models.Any(x=>x.Energy>=50))
            {
                throw new InvalidOperationException();
            }
            var workShop = new Workshop();
            var egg = eggs.Models.FirstOrDefault(x => x.Name == eggName);
            foreach (var bunny in bunnies.Models.Where(x => x.Energy >= 50).OrderByDescending(x => x.Energy).Take(1))
            {
                workShop.Color(egg, bunny);

                if (bunny.Energy <= 0)
                {
                    bunnies.Remove(bunny);
                }
            }

    
            if (egg.IsDone())
            {
                coloredEggs++;
                return $"Egg {eggName} is done.";
                
            }
            else
            {
                return $"Egg {eggName} is not done.";
            }
        }

        public string Report()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{coloredEggs} eggs are done!");
            sb.AppendLine("Bunnies info:");
            foreach (var bunny in bunnies.Models)
            {
                sb.AppendLine($"Name: {bunny.Name}");
                sb.AppendLine($"Energy: {bunny.Energy}");
                sb.AppendLine($"Dyes: {bunny.Dyes.Count - bunny.Dyes.Where(x=>x.Power <=0).Count()} not finished");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
