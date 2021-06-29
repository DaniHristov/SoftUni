namespace VaporStore.DataProcessor
{
	using System;
    using System.Linq;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			var games = context.Genres.ToList().Where(x => genreNames.Contains(x.Name)).Select(x => new
			{
				Id = x.Id,
				Genre = x.Name,
				Games = x.Games.Select(g => new
				{
					Id = g.Id,
					Title = g.Name,
					Developer = g.Developer.Name,
					Tags = string.Join(", ", g.GameTags.Select(gt => gt.Tag.Name)),
					Players = g.Purchases.Count(),
				})
				.Where(x=>x.Players > 0).OrderByDescending(x=>x.Players).ThenBy(x=>x.Id),
				TotalPlayers = x.Games.Sum(x=>x.Purchases.Count)
			}).OrderByDescending(x=>x.TotalPlayers).ThenBy(x=>x.Id);

			var result = JsonConvert.SerializeObject(games,Formatting.Indented);
			return result;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			var users = context.Users.ToList().Where(u => u.Cards.Any(c => c.Purchases.Any())).Select(x => new UserPurchasesExportModel
			{
				Username = x.Username,
				Purchases = x.Cards.Select(p=> new PurchaseExportModel
                {
					Card = p.Number,
					Cvc = p.Cvc,
				    Date = 
                })
			})

			return "TODO";
		}
	}
}