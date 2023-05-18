using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class ApplicationField
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string FieldName { get; set; } = null!;
        public string? Note { get; set; }
        public byte FieldType { get; set; }
        public short WordLimit { get; set; }
        public short RowLimit { get; set; }
        public short FileSizeLimit { get; set; }
        public bool IsRequired { get; set; }
        public bool IsIncludedExport { get; set; }
        public bool IsFixed { get; set; }
        public int Sort { get; set; }
        public string Creator { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string? Updator { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
