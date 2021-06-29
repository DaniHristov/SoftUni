using Easter.Models.Bunnies.Contracts;
using Easter.Models.Eggs.Contracts;
using Easter.Models.Workshops.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easter.Models.Workshops
{
    public class Workshop : IWorkshop
    {
        public Workshop()
        {

        }

        public void Color(IEgg egg, IBunny bunny)
        {
            if (bunny.Dyes.Any(x=>x.Power>0)  &&  bunny.Energy > 0)
            {
                foreach (var dye in bunny.Dyes)
                {
                    while (dye.Power > 0 && egg.EnergyRequired > 0 && bunny.Energy > 0)
                    {
                        dye.Use();
                        egg.GetColored();
                        bunny.Work();
                        if (egg.IsDone())
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
