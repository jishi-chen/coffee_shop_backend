using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Model.DTO
{
    public class DocumentRecordDetailDTO
    {

        public string? DocumentRecordId { get; set; }

        public string? DocumentFieldId { get; set; }

        public string? FilledText { get; set; }

        public string? MemoText { get; set; }

        public string? Remark { get; set; }
    }
}
