using System.ComponentModel.DataAnnotations;

public class CreateProductDto
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string SKU { get; set; } = string.Empty;
    [Range(0, int.MaxValue)] public int Quantity { get; set; }
    [Range(0, int.MaxValue)] public int ReorderLevel { get; set; }
    [Required] public int SupplierId { get; set; }
}
