using coffee_shop_backend.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace coffee_shop_backend.ViewModels
{
    public class DocumentViewModel
    {
        public DocumentInfoPage InfoPage { get; set; } = new DocumentInfoPage();
        public DocumentQuestionPage QuestionPage { get; set; } = new DocumentQuestionPage();
    }

    public class DocumentInfoPage
    {
        public string? Id { get; set; } = string.Empty;
        public string CsId { get; set; } = string.Empty;
        public string Caption { get; set; } = null!;
        public bool IsEnabled { get; set; }
        public int Sort { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? HeadText { get; set; }
        public string? FooterText { get; set; }
    }
    public class DocumentQuestionPage
    {
        public string? DocumentFieldId { get; set; }
        public string? ParentId { get; set; }
        public string Caption { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public bool IsIncludedExport { get; set; }
        public bool IsEditable { get; set; }
        public string? Note { get; set; }
        public int FileSizeLimit { get; set; } = 0;
        public int WordLimit { get; set; } = 0;
        public int AnswerType { get; set; } = 0;
        public string? FileExtension { get; set; } = string.Empty;
        public AnswerOption NewOption { get; set; } = new AnswerOption();
        public List<AnswerOption> OptionList { get; set; } = new List<AnswerOption>();
        public List<DocumentField> FieldList { get; set; } = new List<DocumentField>();
        public List<SelectListItem> AnswerTypeList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MemoTypeList { get; set; } = new List<SelectListItem>();
    }

    public class AnswerOption
    {
        public string Text { get; set; } = string.Empty;
        public int? Sort { get; set; }
        public int MemoType { get; set; }
        public string MemoText { get; set; } = string.Empty;
    }
}
