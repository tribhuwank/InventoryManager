using InventoryManager.API.Entities;

namespace InventoryManager.API.ViewModel
{
    public class ProductViewModel : BaseEntity
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
    }
}
