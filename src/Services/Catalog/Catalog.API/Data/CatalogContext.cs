using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
	public class CatalogContext : ICatalogContext
	{
		private MongoClient? _client;
		private IMongoDatabase _database;
		public IMongoCollection<Product> Products { get; }

		public CatalogContext(IConfiguration configuration)
		{
			_client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			_database = _client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

			Products = _database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
			SeedMongoData();
		}

		public async Task SeedMongoData()
		{
			await CatalogContextSeed.SeedDataAsync(Products);
		}
		
	}
}
