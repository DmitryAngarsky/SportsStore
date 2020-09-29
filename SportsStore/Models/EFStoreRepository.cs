using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EfStoreRepository : IStoreRepository
    {
        private readonly StoreDbContext _context;

        public EfStoreRepository(StoreDbContext ctx)
        {
            _context = ctx;
        }

        public IQueryable<Product> Products => _context.Products;
        
        public async Task SaveProduct(Product p)
        {
            await _context.AddAsync(p);
            await _context.SaveChangesAsync();
        }

        public async Task CreateProduct(Product p)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Product p)
        {
            _context.Remove(p);
            await _context.SaveChangesAsync();
        }
    }
}
