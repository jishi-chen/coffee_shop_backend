using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Model.ViewModels
{
    public class FileUploadViewModel
    {
        public IFormFile File { get; set; } = null!;
        public string? Description { get; set; }
    }
}
