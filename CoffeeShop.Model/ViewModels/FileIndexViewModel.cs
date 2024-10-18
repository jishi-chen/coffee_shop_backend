using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Model.ViewModels
{
    public class FileIndexViewModel
    {
        public int FileStorageId { get; set; }

        public string OriginalFileName { get; set; } = null!;

        public string NewFileName { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public long? FileSize { get; set; }

        public string? ContentType { get; set; }

        public DateTime UploadDate { get; set; }

        public string? ModuleType { get; set; }

        public string? CategoryType { get; set; }

        public string? Description { get; set; }

        public bool IsDeleted { get; set; }
    }
}
