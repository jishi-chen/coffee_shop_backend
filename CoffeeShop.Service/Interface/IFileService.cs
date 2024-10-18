using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Utility.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Service.Interface
{
    public interface IFileService
    {
        IEnumerable<FileStorage> GetAll(string? searchString);
        Task<bool> UploadFileAsync(FileUploadViewModel model);
        bool DeleteFile(int fileStorageId);
        byte[] DownloadFile(int fileStorageId);
        string[] GetFiles();
    }
}
