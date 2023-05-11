namespace coffee_shop_backend.ViewModels
{
    public class QuestionnaireViewModel
    {
        public Guid Id { get; set; }
        public string Caption { get; set; } = null!;
        public bool IsEnabled { get; set; }
        public short Sort { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? HeadText { get; set; }
        public string? FooterText { get; set; }
        public int Hits { get; set; }
    }
}
