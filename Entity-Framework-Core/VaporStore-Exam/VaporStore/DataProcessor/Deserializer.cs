namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dto.Import;
	using System.Globalization;
	using System.Linq;
    using VaporStore.Data.Models;
    using System.Text;
    using System.Xml.Serialization;
    using System.IO;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			var Games = JsonConvert.DeserializeObject<IEnumerable<GamesImportModel>>(jsonString);
			var sb = new StringBuilder();
			foreach (var game in Games)
			{
				if (!IsValid(game) || !game.Tags.Any())
				{
					sb.AppendLine("Invalid Data");
					continue;
				}

				var genre = context.Genres.FirstOrDefault(x => x.Name == game.Genre) 
					?? new Genre { Name = game.Genre };
				var developer = context.Developers.FirstOrDefault(x => x.Name == game.Developer)
					?? new Developer { Name = game.Developer };

				var newGame = new Game
				{
					Name = game.Name,
					Price = game.Price,
					ReleaseDate = game.ReleaseDate.Value,
					Developer = developer,
					Genre = genre
				};

                foreach (var gameTag in game.Tags)
                {
					var tag = context.Tags.FirstOrDefault(x => x.Name == gameTag) 
						?? new Tag { Name = gameTag };

					newGame.GameTags.Add(new GameTag { Tag = tag });
                }
				context.Games.Add(newGame);
				context.SaveChanges();
                sb.AppendLine($"Added {game.Name} ({game.Genre}) with {game.Tags.Count()} tags");
            }
			return sb.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			var users = JsonConvert.DeserializeObject<IEnumerable<UsersImportModel>>(jsonString);
			var sb = new StringBuilder();
            foreach (var user in users)
            {
                if (!IsValid(user))
                {
					sb.AppendLine($"Invalid Data");
					continue;
                }
				var userObj = new User()
				{
					FullName = user.FullName,
					Username = user.Username,
					Email = user.Email,
					Age = user.Age,
					Cards = user.Cards.Select(x=>new Card
                    {
						Number = x.Number,
						Cvc = x.CVC,
						Type = x.Type.Value
                    })
					.ToList()
				};
				context.Users.Add(userObj);
				context.SaveChanges();
				sb.AppendLine($"Imported {user.Username} with {user.Cards.Count()} cards");
            }
			return sb.ToString().TrimEnd();
			
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			//var purchases = XmlConverter.Deserializer<IEnumerable<PurchasesXmlInputModel>>(xmlString, "Purchases");

			XmlSerializer serializer = new XmlSerializer(typeof(PurchasesXmlInputModel[]), new XmlRootAttribute("Purchases"));
			var purchases = (PurchasesXmlInputModel[])serializer.Deserialize(new StringReader(xmlString));
			var sb = new StringBuilder();
            foreach (var purchase in purchases)
            {
				bool dateParse = DateTime.TryParseExact(purchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);

				if (!IsValid(purchase))
                {
					sb.AppendLine("Invalid Data");
					continue;
                }

				var xmlPurchase = new Purchase
				{
					Date = date,
					ProductKey = purchase.Key,
					Type = purchase.Type.Value
				};

				xmlPurchase.Card = context.Cards.FirstOrDefault(x => x.Number == purchase.Card);
				xmlPurchase.Game = context.Games.FirstOrDefault(x => x.Name == purchase.Title);

				context.Purchases.Add(xmlPurchase);
				context.SaveChanges();
				var username = context.Users.Where(x => x.Id == xmlPurchase.Card.UserId).Select(x=>x.Username).FirstOrDefault();
				sb.AppendLine($"Imported {purchase.Title} for {username}");
            }

			return sb.ToString().TrimEnd();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}