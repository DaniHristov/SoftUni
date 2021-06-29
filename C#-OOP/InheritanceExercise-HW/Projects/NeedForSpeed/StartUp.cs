using System;

namespace NeedForSpeed
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Car car = new Car(200,100);
            car.Drive(15);
            Console.WriteLine(car.Fuel);
        }
    }
}
