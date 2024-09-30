
namespace CoffeeShop.Model.ViewModels
{
    public class TenantFormViewModel
    {
        public int? TenantId { get; set; }
        public string TenantName { get; set; } = null!;
        public string? Address { get; set; }
        public string ContactEmail { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public bool IsEnabled { get; set; }
    }
}
