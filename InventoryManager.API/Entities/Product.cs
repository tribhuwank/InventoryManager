using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventoryManager.API.Entities
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        [Precision(14,2)]
        public decimal Price { get; set; }
        public bool Status { get; set; }  //product in stock or not
        public bool IsDelete { get; set; }  // for soft delete
    }
}
