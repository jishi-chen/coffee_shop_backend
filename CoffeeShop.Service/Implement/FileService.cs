using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Implement;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using CoffeeShop.Utility.Helpers;
using Microsoft.AspNetCore.Http;


namespace CoffeeShop.Service.Implement
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUtility _fileUtility = new FileUtility();
        private readonly IUserService _userService;

        public FileService(IUserService userService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public IEnumerable<FileStorage> GetAll(string? searchString)
        {
            var result = _unitOfWork.FileRepository.GetAll().Where(x => x.IsDeleted == false);
            _unitOfWork.Dispose();
            if (searchString != null)
            {
                result = result.Where(x => x.OriginalFileName.Contains(searchString)).ToList();
            }
            return result;
        }

        public async Task<bool> UploadFileAsync(FileUploadViewModel model)
        {
            IFormFile file = model.File;
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
            if (file != null && file.Length > 0)
            {
                FileStorage fileStorage = new FileStorage()
                {
                    NewFileName = newFileName,
                    OriginalFileName = file.FileName,
                    ContentType = file.ContentType,
                    FilePath = _fileUtility.GetFilePath(newFileName),
                    FileSize = file.Length,
                    ModuleType = "File",
                    CategoryType = "File",
                    Description = model.Description,
                    UploadedBy = _userService.GetCurrentLoginId(),
                    IsDeleted = false,
                };
                _fileUtility.allowedExtensions = ".xlsx,.jpg,.png";
                if (await _fileUtility.UploadFileAsync(fileStorage.NewFileName, file.OpenReadStream()))
                {
                    _unitOfWork.FileRepository.Add(fileStorage);
                    _unitOfWork.Complete();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteFile(int fileStorageId)
        {
            var fileStorage = _unitOfWork.FileRepository.GetById(fileStorageId);
            if (fileStorage != null)
            {
                if (_fileUtility.DeleteFile(fileStorage.NewFileName))
                {
                    fileStorage.IsDeleted = true;
                    _unitOfWork.FileRepository.Update(fileStorage);
                    _unitOfWork.Complete();
                    return true;
                }
            }
            return false;
        }
        public byte[] DownloadFile(int fileStorageId)
        {
            var fileStorage = _unitOfWork.FileRepository.GetById(fileStorageId);
            _unitOfWork.Dispose();
            if (fileStorage == null)
            {
                throw new FileNotFoundException("File not found");
            }
            return _fileUtility.ReadFileBytes(fileStorage.NewFileName);
        }

        public string[] GetFiles()
        {
            return _fileUtility.GetFiles();
        }
    }
}
