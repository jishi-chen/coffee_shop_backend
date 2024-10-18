using System.IO;
using System.IO.Compression;

namespace CoffeeShop.Utility.Helpers
{
    public class FileUtility
    {
        public string allowedExtensions { get; set; } = ".odt,.ods";
        private readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>
         {
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".jpeg", "image/jpeg" },
            { ".pdf", "application/pdf" },
            { ".ods", "application/vnd.oasis.opendocument.spreadsheet" },
            { ".odt", "application/vnd.oasis.opendocument.text" },
            { ".mp4", "video/mp4" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            { ".zip", "application/zip" }
         };
        private readonly Dictionary<string, string> ValidMagicNumbers = new Dictionary<string, string>
         {
            { ".jpg", "FFD8" },
            { ".jpeg", "FFD8" },
            { ".png", "8950" },
            { ".gif", "4749" },
            { ".pdf", "2550"},
            { ".ods", "504B" },
            { ".odt", "504B" },
            { ".mp4", "0000" },
            { ".xlsx", "504B" },
            { ".zip", "504B" }
         };

        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        /// <summary>
        /// 檔案上傳
        /// </summary>
        /// <param name="fileName">檔案名稱</param>
        /// <param name="fileStream">fileStream</param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(string fileName, Stream fileStream)
        {
            try
            {
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }
                var filePath = Path.Combine(_uploadPath, GetFilePath(fileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    string errMsg = CheckIsUploadFilesValid(fileStream, Path.GetExtension(fileName));
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        return false;
                    }
                    await fileStream.CopyToAsync(stream);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool DeleteFile(string fileName)
        {
            return DeleteFile("", fileName);
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="fileName">檔案名稱</param>
        /// <returns></returns>
        public bool DeleteFile(string filePath, string fileName)
        {
            if (!string.IsNullOrEmpty(filePath))
                filePath = Path.Combine(filePath, filePath);
            else
                filePath = Path.Combine(_uploadPath, fileName);
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string[] GetFiles()
        {
            try
            {
                return Directory.GetFiles(_uploadPath);
            }
            catch
            {
                return new string[0]; // 返回空陣列代表沒有檔案
            }
        }

        /// <summary>
        /// 讀取檔案
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] ReadFileBytes(string fileName)
        {
            return ReadFileBytes("", fileName);
        }

        /// <summary>
        /// 讀取檔案
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="fileName">檔案名稱</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public byte[] ReadFileBytes(string filePath, string fileName)
        {
            if (!string.IsNullOrEmpty(filePath))
                filePath = Path.Combine(filePath, filePath);
            else
                filePath = Path.Combine(_uploadPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }
            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// 檢查檔案格式
        /// </summary>
        /// <param name="file"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string CheckIsUploadFilesValid(Stream file, string extension)
        {
            List<string> AllowedExtensions = allowedExtensions.ToLower().Split(',').ToList();
            if (file == null)
            {
                return "檔案不存在。";
            }

            // 檢查副檔名
            if (!AllowedExtensions.Contains(extension))
            { return extension + "為不合法的檔案格式。"; }

            // 檢查 MIME 類型
            if (!MimeTypes.ContainsKey(extension))
            { return MimeTypes[extension] + "為不合法的檔案格式。"; }

            // 檢查魔數
            byte[] fileBytes = new byte[2];
            file.Read(fileBytes, 0, 2);
            string fileMagicNumber = ((int)fileBytes[0]).ToString("X2") + ((int)fileBytes[1]).ToString("X2");
            //檢查壓縮檔
            if (extension == ".zip")
            {
                string strErrMsg = ProcessUploadedZip(file);
                if (!string.IsNullOrEmpty(strErrMsg))
                {
                    return strErrMsg;
                }
            }
            if (ValidMagicNumbers[extension] != fileMagicNumber)
            { return "檔案格式錯誤。"; }

            //歸位
            file.Position = 0;
            return string.Empty;
        }
        public string ProcessUploadedZip(Stream zipStream)
        {
            if (zipStream != null && zipStream.Length > 0)
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        using (var entryStream = entry.Open())
                        {
                            return CheckIsUploadFilesValid(entryStream, Path.GetExtension(entry.Name).ToLower());
                        }
                    }
                }
            }
            return "上傳的檔案無效或內容為空。";
        }
        public string GetFilePath(string fileName)
        {
            return GetFilePath("", fileName);
        }

        public string GetFilePath(string moduleType, string fileName)
        {
            if (moduleType == string.Empty)
            {
                return fileName;
            }
            return Path.Combine(moduleType, fileName);
        }
    }
}
