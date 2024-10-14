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
            { ".zip", "504B" }
         };

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
    }
}
