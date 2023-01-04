using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
	public interface IProductRepository
	{
		Task <IEnumerable<Product>> GetProductsAsync();
		Task<Product> GetProductByIdAsync(string id);

		Task<IEnumerable<Product>> GetProductByNameAsync(string productName);
		Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName);

		Task CreateProduct(Product product);
		Task<bool> UpdateProduct(Product product);
		Task<bool> DeleteProduct(string id);
	}
}
