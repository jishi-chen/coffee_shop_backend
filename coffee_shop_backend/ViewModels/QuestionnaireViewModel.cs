using coffee_shop_backend.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace coffee_shop_backend.ViewModels
{
    public class QuestionnaireViewModel
    {
        public BasicInformation Info { get; set; } = new BasicInformation();
        public QuestionContent Question { get; set; } = new QuestionContent();
    }

    public class BasicInformation
    {
        public Guid Id { get; set; }
        public string Caption { get; set; } = null!;
        public bool IsEnabled { get; set; }
        public int Sort { get; set; }
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string? HeadText { get; set; }
        public string? FooterText { get; set; }
        public int Hits { get; set; }
    }
    public class QuestionContent
    {
        public Guid QuestionId { get; set; }
        public string Caption { get; set; } = string.Empty;
        public bool IsNeedAnswer { get; set; }
        public bool IsIncludedExport { get; set; }
        public string? Note { get; set; }
        public int FileSizeLimit { get; set; } = 0;
        public int WordLimit { get; set; } = 0;
        public int AnswerType { get; set; } = 0;
        public List<SelectListItem> AnswerTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MemoTypes { get; set; } = new List<SelectListItem>();
        public AnswerOption NewOption { get; set; } = new AnswerOption();
        public List<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
        public List<ApplicationField> QuestionList { get; set; } = new List<ApplicationField>();
    }
    public class AnswerOption
    {
        public string Text { get; set; } = string.Empty;
        public int? Sort { get; set; }
        public int MemoType { get; set; }
        public string MemoText { get; set; } = string.Empty;
    }
}
