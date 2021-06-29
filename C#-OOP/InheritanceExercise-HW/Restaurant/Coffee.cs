using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Restaurant
{
    class Coffee : HotBeverage
    {
        private const double coffeeMilliliters = 50;

        private const decimal coffeePrice = 3.50m;

        public Coffee(string name,double caffeine) : base(name, coffeePrice, coffeeMilliliters)
        {
            this.Caffeine = caffeine;
        }

        public double Caffeine { get; set; }
    }
}
