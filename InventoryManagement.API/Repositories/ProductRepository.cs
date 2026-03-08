using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products.Include(p => p.Supplier).ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products.Include(p => p.Supplier).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<bool> SkuExistsAsync(string sku) =>
        await _context.Products.AnyAsync(p => p.SKU == sku);

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetLowStockAsync() =>
        await _context.Products
            .Where(p => p.Quantity <= p.ReorderLevel)
            .Include(p => p.Supplier)
            .ToListAsync();
}
