using coffee_shop_backend.Enums;

namespace coffee_shop_backend.ViewModels
{
    public class DocumentFormViewModel
    {
        public string Id { get; set; } = null!;
        public string Caption { get; set; } = string.Empty;
        public string? HeadText { get; set; } = string.Empty;
        public string? FooterText { get; set; } = string.Empty;
        /// <summary> 問題集 </summary>
        public List<DocumentFieldViewModel> Fields { get; set; } = new List<DocumentFieldViewModel>();
        public Dictionary<string, string> ValidResults { get; set; } = new Dictionary<string, string>();
    }

    public class DocumentFieldViewModel
    {
        public string Id { get; set; } = null!;
        public string? ParentId { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;
        public AnswerTypeEnum FieldType { get; set; }
        public int WordLimit { get; set; }
        public int RowLimit { get; set; }
        public int FileSizeLimit { get; set; }
        public string? FileExtension { get; set; } = string.Empty;
        public int Sort { get; set; }
        public bool IsRequired { get; set; }
        public bool IsIncludedExport { get; set; }
        public bool IsEditable { get; set; }
        public List<DocumentFieldOptionViewModel> Options { get; set; } = new List<DocumentFieldOptionViewModel>();
        public string? Value { get; set; } = string.Empty;
        public string? MemoValue { get; set; } = string.Empty;
        public string? Remark { get; set; } = string.Empty;
        public string ErrMsg { get; set; } = string.Empty;
    }

    /// <summary> 問卷選項題目 </summary>
    public class DocumentFieldOptionViewModel
    {
        public string Id { get; set; } = null!;
        public string OptionName { get; set; } = string.Empty;
        public int Sort { get; set; }
        public MemoTypeEnum MemoType { get; set; }
        public bool Checked { get; set; }
        public string? MemoValue { get; set; } = string.Empty;
    }
}
