namespace coffee_shop_backend.ViewModels
{
    public class RecordViewModel
    {
        public Guid FieldID { get; set; }
        public string FieldName { get; set; }
        public string FilledText { get; set; }
        public int Sort { get; set; }
        public byte FieldType { get; set; }
        public bool IsRequired { get; set; }
        public bool IsFixed { get; set; }
        public string Value { get; set; }
        public string MemoValue { get; set; }
        public string Remark { get; set; }
    }
}
