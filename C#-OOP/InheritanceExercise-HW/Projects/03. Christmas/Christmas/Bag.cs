using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Christmas
{
    class Bag
    {
        private List<Present> data;

        public Bag(string color , int capacity)
        {
            Color = color;
            Capacity = capacity;
            data = new List<Present>();
        }

        public string Color { get; set; }

        public int Capacity { get; set; }

        public int Count { get { return data.Count; } }
        public void Add(Present present)
        {
            if (Capacity>data.Count)
            {
                data.Add(present);
            }
        }

        public bool Remove(string name)
        {
            Present present = data.FirstOrDefault(p => p.Name == name);
            if (present!=null)
            {
                data.Remove(present);
                return true;
            }
            return false;
        }

        public Present GetHeaviestPresent()
        {
            Present heaviest = data.OrderByDescending(p => p.Weight).FirstOrDefault();
            return heaviest;
        }

        public Present GetPresent(string name)
        {
            Present present = data.FirstOrDefault(p => p.Name == name);

            return present;
            
        }

        public string Report()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{Color} bag contains:");
            foreach (var present in data)
            {
                result.AppendLine(present.ToString());
            }

            return result.ToString().Trim();
        }
    }
}
