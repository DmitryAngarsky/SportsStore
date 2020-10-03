using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }

        Task SaveProductAsync(Product p);
        Task CreateProductAsync(Product p);
        Task DeleteProductAsync(Product p);
    }
}
