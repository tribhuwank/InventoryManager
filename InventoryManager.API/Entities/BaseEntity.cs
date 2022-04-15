namespace InventoryManager.API.Entities
{
    public abstract class BaseEntity
    {
        public string EntryBy { get; set; } = string.Empty;
        public DateTime EntryOn { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedOn { get; set; }
    }
}
