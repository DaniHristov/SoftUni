using Bakery.Models.BakedFoods;
using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Tables.Contracts;
using Bakery.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bakery.Models.Tables
{
    public abstract class Table : ITable
    {

        private readonly ICollection<IBakedFood> FoodOrders;
        private readonly ICollection<IDrink> DrinkOrders;

        private int capacity;
        private int numberOfPeople;

        protected Table(int tableNumber, int capacity, decimal pricePerPerson)
        {
            this.IsReserved = false;
            this.TableNumber = tableNumber;
            this.Capacity = capacity;
            this.PricePerPerson = pricePerPerson;
            this.FoodOrders = new List<IBakedFood>();
            this.DrinkOrders = new List<IDrink>();

        }

        public int TableNumber { get; }

        public int Capacity
        {
            get { return this.capacity; }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidTableCapacity);
                }

                this.capacity = value;
            }
        }

        public int NumberOfPeople
        {
            get { return this.numberOfPeople; }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidNumberOfPeople);
                }
                this.numberOfPeople = value;
            }
        }

        public decimal PricePerPerson { get; }

        public bool IsReserved { get; private set; }

        public decimal Price
              => this.NumberOfPeople * this.PricePerPerson;

        public void Clear()
        {
            this.IsReserved = false;
            this.FoodOrders.Clear();
            this.DrinkOrders.Clear();
            this.Capacity = 0;


        }

        public decimal GetBill()
        {
            decimal totalSum = FoodOrders.Sum(f => f.Price) + DrinkOrders.Sum(d => d.Price) + Price;

            return totalSum;
        }

        public string GetFreeTableInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Table: {this.TableNumber}");
            sb.AppendLine($"Type: {this.GetType().Name}");
            sb.AppendLine($"Capacity: {this.Capacity}");
            sb.AppendLine($"Price per Person: {this.PricePerPerson}");

            return sb.ToString().TrimEnd();
        }

        public void OrderDrink(IDrink drink)
        {
                this.DrinkOrders.Add(drink);
        }

        public void OrderFood(IBakedFood food)
        {
                this.FoodOrders.Add(food);
        }

        public void Reserve(int numberOfPeople)
        {
            if (numberOfPeople >= this.Capacity)
            {
                this.IsReserved = true;
                this.NumberOfPeople = numberOfPeople;
            }
        }
    }
}
