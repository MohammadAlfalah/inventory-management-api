public interface ISupplierService
{
    Task<IEnumerable<SupplierReadDto>> GetAllAsync();
    Task<SupplierReadDto?> GetByIdAsync(int id);
    Task<(bool Success, string Error)> CreateAsync(CreateSupplierDto dto);
    Task<bool> DeleteAsync(int id);
}
