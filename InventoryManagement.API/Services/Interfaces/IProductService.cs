public interface IProductService
{
    Task<IEnumerable<ProductReadDto>> GetAllAsync();
    Task<ProductReadDto?> GetByIdAsync(int id);
    Task<(bool Success, string Error)> CreateAsync(CreateProductDto dto);
    Task<(bool Success, string Error)> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ProductReadDto>> GetLowStockAsync();
}
