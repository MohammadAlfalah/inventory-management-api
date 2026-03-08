using Microsoft.EntityFrameworkCore;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;
    public SupplierRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Supplier>> GetAllAsync() =>
        await _context.Suppliers.ToListAsync();

    public async Task<Supplier?> GetByIdAsync(int id) =>
        await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);

    public async Task AddAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Supplier supplier)
    {
        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
    }
}
