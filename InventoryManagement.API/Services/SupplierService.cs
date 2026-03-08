public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repo;
    public SupplierService(ISupplierRepository repo) => _repo = repo;

    private static SupplierReadDto ToDto(Supplier s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Email = s.Email,
        Phone = s.Phone
    };

    public async Task<IEnumerable<SupplierReadDto>> GetAllAsync() =>
        (await _repo.GetAllAsync()).Select(ToDto);

    public async Task<SupplierReadDto?> GetByIdAsync(int id)
    {
        var supplier = await _repo.GetByIdAsync(id);
        return supplier is null ? null : ToDto(supplier);
    }

    public async Task<(bool Success, string Error)> CreateAsync(CreateSupplierDto dto)
    {
        var supplier = new Supplier
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone
        };

        await _repo.AddAsync(supplier);
        return (true, string.Empty);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var supplier = await _repo.GetByIdAsync(id);
        if (supplier is null) return false;
        await _repo.DeleteAsync(supplier);
        return true;
    }
}
