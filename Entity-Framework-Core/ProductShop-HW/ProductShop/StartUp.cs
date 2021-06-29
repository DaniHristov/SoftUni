using ProductShop.Data;
using System.IO;
using ProductShop.Dtos.Import;
using System.Linq;
using ProductShop.Models;
using System;
using ProductShop.Dtos.Export;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var inputUsers = File.ReadAllText("../../../Datasets/users.xml");
            //var inputProducts = File.ReadAllText("../../../Datasets/products.xml");
            //var inputCategories = File.ReadAllText("../../../Datasets/categories.xml");
            //var inputCategoriesProdcuts = File.ReadAllText("../../../Datasets/categories-products.xml");
            //ImportUsers(context,inputUsers);
            //ImportProducts(context, inputProducts);
            //ImportCategories(context,inputCategories);
            //Console.WriteLine(ImportCategoryProducts(context,inputCategoriesProdcuts));
            //Console.WriteLine(GetProductsInRange(context));
            //Console.WriteLine(GetSoldProducts(context));
            //Console.WriteLine(GetCategoriesByProductsCount(context));
            Console.WriteLine(GetCategoriesByProductsCount(context));

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = new UserCountDto()
            {
                Count = context.Users.Count(),
                Users = context.Users
                .Where(x => x.ProductsSold.Count > 0)
                .OrderByDescending(x => x.ProductsSold.Count)
                .Select(u => new UserExportDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age.ToString(),
                    SoldProducts = new SoldProductsCountDto()
                    {
                        Count = u.ProductsSold.Count,
                        SoldProducts = u.ProductsSold.Select(x => new SoldProductsOutput()
                        {
                            Name = x.Name,
                            Price = x.Price
                        })
                        .ToArray()
                    }
                }).ToArray()
            };

            var result = XmlConverter.XmlConverter.Serialize(users, "Users");
            return result;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new CategoriesOutputModel
            {
                Name = c.Name,
                Count = c.CategoryProducts.Count,
                AveragePrice = c.CategoryProducts
                .Average(x => x.Product.Price),
                TotalRevenue = c.CategoryProducts
                .Sum(x => x.Product.Price)
            })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToList();

            var result = XmlConverter.XmlConverter.Serialize(categories, "Categories");
            return result;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .Select(u => new SoldProductsOuputModel
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(x => new SoldProductsOutput()
                    {
                        Name = x.Name,
                        Price = x.Price
                    }).ToArray()
                }).OrderBy(x => x.LastName).ThenBy(x => x.LastName).Take(5).ToList();

            var result = XmlConverter.XmlConverter.Serialize(soldProducts, "Users");
            return result;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductsInRangeOutputModel
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToList();

            var result = XmlConverter.XmlConverter.Serialize(productsInRange,"Products");
            return result;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            const string root = "CategoryProducts";

            var categoryProductsDto = XmlConverter.XmlConverter.Deserializer<CategoryProductInputModel>(inputXml, root);

            var productIds = context.Products.Select(x => x.Id).ToList();

            var categoryProdcuts = categoryProductsDto.Where(x => productIds.Contains(x.ProductId)).Select(x => new CategoryProduct
            {
                CategoryId = x.CategoryId,
                ProductId = x.ProductId
            })
                .ToList();

            context.CategoryProducts.AddRange(categoryProdcuts);
            context.SaveChanges();
            return $"Successfully imported {categoryProdcuts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            const string root = "Categories";

            var categoriesDto = XmlConverter.XmlConverter.Deserializer<CategoryInputModel>(inputXml, root);

            var categories = categoriesDto.Where(x=>x.Name !=null).Select(x => new Category
            {
                Name = x.Name
            })
                .ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count}";

        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            const string root = "Products";

            var productsDto = XmlConverter.XmlConverter.Deserializer<ProductInputModel>(inputXml, root);

            var products = productsDto
                .Select(x => new Product
            {
                Name = x.Name,
                Price = x.Price,
                SellerId = x.SellerId,
                BuyerId = x.BuyerId,
            })
                .ToList();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            const string root = "Users";

            var usersDto = XmlConverter.XmlConverter.Deserializer<UserInputModel>(inputXml,root);

            var users = usersDto
                .Select(x => new User
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Age = x.Age
            })
                .ToList();

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}
//StringBuilder sb = new StringBuilder();
//XmlSerializer serializer = new XmlSerializer(typeof(SellerUserDto[]), new XmlRootAttribute("users"));
//XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
//serializer.Serialize(new StringWriter(sb), users, xmlNamespaces);