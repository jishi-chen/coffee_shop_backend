using CoffeeShop.Model.DTO;
using CoffeeShop.Model.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CoffeeShop.Model.ViewModels
{
    public class DocumentViewModel
    {
        public DocumentDTO InfoPage { get; set; } = new DocumentDTO();
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
        public List<DocumentFieldDTO> FieldList { get; set; } = new List<DocumentFieldDTO>();
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

    public class DocumentDTO
    {
        public string Id { get; set; } = null!;

        public string Caption { get; set; } = null!;

        public bool IsEnabled { get; set; }

        public int Sort { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? HeadText { get; set; }

        public string? FooterText { get; set; }

        public int Hits { get; set; }

        public string Creator { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public string? Updator { get; set; }

        public DateTime? UpdateDate { get; set; }
    }

}
