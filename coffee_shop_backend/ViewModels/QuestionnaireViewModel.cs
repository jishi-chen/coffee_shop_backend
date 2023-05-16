using System;
using coffee_shop_backend.Enums;
using coffee_shop_backend.Utility;
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
        public short Sort { get; set; }
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string? HeadText { get; set; }
        public string? FooterText { get; set; }
        public int Hits { get; set; }
    }
    public class QuestionContent
    {
        public string? Caption { get; set; }
        public bool IsNeedAnswer { get; set; }
        public bool IsIncludedExport { get; set; }
        public string? Note { get; set; }
        public int FileSizeLimit { get; set; } = 0;
        public int WordLimit { get; set; } = 0;
        public List<SelectListItem> AnswerType { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MemoTypes { get; set; } = new List<SelectListItem>();
        public AnswerOption NewOption { get; set; } = new AnswerOption();
        public List<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    }
    public class AnswerOption
    {
        public string? Text { get; set; } = string.Empty;
        public int? Sort { get; set; }
        public int MemoType { get; set; }
    }
}
