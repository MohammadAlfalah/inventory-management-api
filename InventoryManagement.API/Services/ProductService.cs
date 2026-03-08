public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    public ProductService(IProductRepository repo) => _repo = repo;

    private static ProductReadDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        SKU = p.SKU,
        Quantity = p.Quantity,
        ReorderLevel = p.ReorderLevel,
        SupplierName = p.Supplier?.Name ?? "Unknown",
        CreatedAt = p.CreatedAt
    };

    public async Task<IEnumerable<ProductReadDto>> GetAllAsync() =>
        (await _repo.GetAllAsync()).Select(ToDto);

    public async Task<ProductReadDto?> GetByIdAsync(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        return product is null ? null : ToDto(product);
    }

    public async Task<(bool Success, string Error)> CreateAsync(CreateProductDto dto)
    {
        if (await _repo.SkuExistsAsync(dto.SKU))
            return (false, "SKU already exists.");

        var product = new Product
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Quantity = dto.Quantity,
            ReorderLevel = dto.ReorderLevel,
            SupplierId = dto.SupplierId
        };

        await _repo.AddAsync(product);
        return (true, string.Empty);
    }

    public async Task<(bool Success, string Error)> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product is null) return (false, "Product not found.");

        if (dto.Name is not null) product.Name = dto.Name;
        if (dto.Quantity is not null) product.Quantity = dto.Quantity.Value;
        if (dto.ReorderLevel is not null) product.ReorderLevel = dto.ReorderLevel.Value;
        if (dto.SupplierId is not null) product.SupplierId = dto.SupplierId.Value;

        await _repo.UpdateAsync(product);
        return (true, string.Empty);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product is null) return false;
        await _repo.DeleteAsync(product);
        return true;
    }

    public async Task<IEnumerable<ProductReadDto>> GetLowStockAsync() =>
        (await _repo.GetLowStockAsync()).Select(ToDto);
}
