namespace coffee_shop_backend.ViewModels
{
    public class DocumentRecordViewModel
    {
        public string? ParentId { get; set; }
        public string FieldId { get; set; } = null!;
        public string FieldName { get; set; } = null!;
        public string FilledText { get; set; } = string.Empty;
        public int Sort { get; set; }
        public byte FieldType { get; set; }
        public bool IsRequired { get; set; }
        public bool IsEditable { get; set; }
        public string? Value { get; set; }
        public string? MemoValue { get; set; } 
        public string? Remark { get; set; }
    }
}
