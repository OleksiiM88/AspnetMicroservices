using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ICatalogContext _context;

		public ProductRepository(ICatalogContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			return await _context.Products.Find(p => true).ToListAsync();
		}

		public async Task<Product> GetProductByIdAsync(string id)
		{
			return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
			return await _context.Products.Find(filter).ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductByNameAsync(string productName)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, productName);
			return await _context.Products.Find(filter).ToListAsync();
		}

		public Task CreateProduct(Product product)
		{
			return _context.Products.InsertOneAsync(product);
		}

		public async Task<bool> DeleteProduct(string id)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
			var result = await _context.Products.DeleteOneAsync(filter);
			return result.IsAcknowledged && result.DeletedCount > 0;
		}

		public async Task<bool> UpdateProduct(Product product)
		{
			var result = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
			return result.IsAcknowledged && result.ModifiedCount > 0;
		}
	}
}
