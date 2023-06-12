using coffee_shop_backend.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace coffee_shop_backend.ViewModels
{
    public class DocumentViewModel
    {
        public Document InfoPage { get; set; } = new Document();
        public DocumentQuestionPage QuestionPage { get; set; } = new DocumentQuestionPage();
    }
    public class DocumentQuestionPage
    {
        public string? DocumentFieldId { get; set; }
        public string? ParentId { get; set; }
        public string Caption { get; set; } = string.Empty;
        public bool IsRequired { get; set; } = true;
        public bool IsIncludedExport { get; set; } = true;
        public bool IsEditable { get; set; } = true;
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
        public List<SelectListItem> ParentFieldList { get; set; } = new List<SelectListItem>();
        public int Layer { get; set; } = 0;
        public bool HasData { get; set; } = false;
    }

    public class AnswerOption
    {
        public string Text { get; set; } = string.Empty;
        public int? Sort { get; set; }
        public int MemoType { get; set; }
        public string MemoText { get; set; } = string.Empty;
    }


}
