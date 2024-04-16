using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Model.DTO
{
    public class DocumentFieldDTO
    {
        public string? Id { get; set; }

        public string? ParentId { get; set; }

        public string? DocumentId { get; set; }

        public string FieldName { get; set; } = null!;

        public string Note { get; set; } = null!;

        public byte FieldType { get; set; }

        public int WordLimit { get; set; }

        public int RowLimit { get; set; }

        public int FileSizeLimit { get; set; }

        public string? FileExtension { get; set; }

        public bool IsRequired { get; set; }

        public bool IsIncludedExport { get; set; }

        public bool IsEditable { get; set; }

        public int Sort { get; set; }

        public string Creator { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public string? Updator { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
