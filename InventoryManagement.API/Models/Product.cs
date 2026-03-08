public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
