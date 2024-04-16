using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Model.DTO
{
    public class DocumentRecordDTO
    {
        public string RegId { get; set; } = null!;

        public string DocumentId { get; set; } = null!;

        public string DocumentFieldId { get; set; } = null!;

        public string FilledText { get; set; } = null!;

        public string MemoText { get; set; } = null!;

        public string Remark { get; set; } = null!;
    }
}
