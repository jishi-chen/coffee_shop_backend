namespace coffee_shop_backend.ViewModels
{
    public class RecordListViewModel
    {
        public Guid RegId { get; set; }
        public Guid ApplicationId { get; set; }
        public string RegName { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
    }
}
