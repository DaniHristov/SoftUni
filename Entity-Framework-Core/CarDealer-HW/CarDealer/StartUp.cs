using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        private static IMapper mapper;
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //var inputSuppliers = File.ReadAllText("../../../Datasets/suppliers.json");
            //var inputParts = File.ReadAllText("../../../Datasets/parts.json");
            //var inputCars = File.ReadAllText("../../../Datasets/cars.json");
            //var inputCustomers = File.ReadAllText("../../../Datasets/customers.json");
            //var inputSales = File.ReadAllText("../../../Datasets/sales.json");
            //ImportSuppliers(context,inputSuppliers);
            //ImportParts(context,inputParts);
            //ImportCars(context,inputCars);
            //ImportCustomers(context,inputCustomers);
            //ImportSales(context, inputSales);
            //Console.WriteLine(GetOrderedCustomers(context));
            //Console.WriteLine(GetCarsFromMakeToyota(context));
            //Console.WriteLine(GetLocalSuppliers(context));
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            //Console.WriteLine(GetTotalSalesByCustomer(context));
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales.Take(10).Select(x => new
            {
                car = new
                {
                    Make = x.Car.Make,
                    Model = x.Car.Model,
                    TravelledDistance = x.Car.TravelledDistance
                },
                customerName = x.Customer.Name,
                Discount = x.Discount.ToString("F2"),
                price = x.Car.PartCars.Sum(c => c.Part.Price).ToString("F2"),
                priceWithDiscount = (x.Car.PartCars.Sum(c => c.Part.Price) - (x.Car.PartCars.Sum(c => c.Part.Price) * (x.Discount / 100))).ToString("F2")

            }).ToList();

            var result = JsonConvert.SerializeObject(sales, Formatting.Indented);
            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var sales = context.Customers.Where(x => x.Sales.Count > 0).Select(x => new
            {
                fullName = x.Name,
                boughtCars = x.Sales.Count,
                spentMoney = x.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
            })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            var result = JsonConvert.SerializeObject(sales, Formatting.Indented);
            return result;

        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars.Select(x => new
            {
                car = new
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,

                },
                parts = x.PartCars.Select(pc => new
                {
                    Name = pc.Part.Name,
                    Price = pc.Part.Price.ToString("F2")
                })
            }).ToList();

            var result = JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);
            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var nativeSuppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                PartsCount = x.Parts.Count()
            })
                .ToList();

            var result = JsonConvert.SerializeObject(nativeSuppliers, Formatting.Indented);
            return result;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotaCars = context.Cars.Where(x => x.Make == "Toyota").Select(x => new
            {
                Id = x.Id,
                Make = x.Make,
                Model = x.Model,
                TravelledDistance = x.TravelledDistance
            }).OrderBy(x => x.Model).ThenByDescending(x => x.TravelledDistance).ToList();

            var result = JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
            return result;
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers.OrderBy(x=>x.BirthDate).ThenBy(x=>x.IsYoungDriver == true).Select(c => new
            {
                Name = c.Name,
                BirthDate = c.BirthDate.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture),
                IsYoungDriver = c.IsYoungDriver
            }).ToList();

            var result = JsonConvert.SerializeObject(customers,Formatting.Indented);
            return result;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();
            var salesDto = JsonConvert.DeserializeObject<IEnumerable<InputSalesModel>>(inputJson);
            var sales = mapper.Map<IEnumerable<Sale>>(salesDto);
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();
            var customersDto = JsonConvert.DeserializeObject<IEnumerable<InputCustomerModel>>(inputJson);
            var customers = mapper.Map<IEnumerable<Customer>>(customersDto);
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();
            var carsDto = JsonConvert.DeserializeObject<IEnumerable<InputCarsModel>>(inputJson);
            var listOfCars = new List<Car>();
            foreach (var car in carsDto)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };

                foreach (var partId in car.PartsId.Distinct())
                {
                    currentCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                listOfCars.Add(currentCar);
            }
            context.Cars.AddRange(listOfCars);
            context.SaveChanges();
            return $"Successfully imported {listOfCars.Count()}.";

        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();
            var partsDto = JsonConvert.DeserializeObject<IEnumerable<InputPartsModel>>(inputJson);
            var parts = mapper.Map<IEnumerable<Part>>(partsDto);
            var partsToAdd = new List<Part>();
            foreach (var part in parts)
            {
                if (part.SupplierId <= 31)
                {
                    partsToAdd.Add(part);
                }
            }
            context.Parts.AddRange(partsToAdd);
            
            context.SaveChanges();
            return $"Successfully imported {partsToAdd.Count()}.";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();
            var supplierDto = JsonConvert.DeserializeObject<IEnumerable<InputSupplierModel>>(inputJson);
            var suppliers = mapper.Map<IEnumerable<Supplier>>(supplierDto);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count()}.";
        }

        private static void InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}