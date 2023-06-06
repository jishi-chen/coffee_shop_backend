using coffee_shop_backend.Enums;
using coffee_shop_backend.Interface;
using coffee_shop_backend.Models;
using coffee_shop_backend.Utility;
using coffee_shop_backend.ViewModels;

namespace coffee_shop_backend.Services
{
    public class DocumentAPI
    {
        private readonly IUnitOfWork _unitOfWork;
        public DocumentAPI(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void FieldDataCheck(IFormCollection collection, IEnumerable<DocumentFieldViewModel> fieldList, Dictionary<string, string> validResultList, string recordId)
        {

            foreach (var field in fieldList)
            {
                string data = collection[field.Id.ToString()];
                string memoData = "";
                string errMsg = "";
                //檢查是否必填
                if (field.IsRequired && string.IsNullOrWhiteSpace(data) && field.FieldType != AnswerTypeEnum.Panel && field.FieldType != AnswerTypeEnum.File)
                    errMsg = $"請輸入{field.FieldName}";
                switch (field.FieldType)
                {
                    case AnswerTypeEnum.Panel:
                        continue;
                    case AnswerTypeEnum.SingleLine:
                    case AnswerTypeEnum.MultiLine:
                        if (field.WordLimit != 0 && data.Length > field.WordLimit)
                            errMsg = $"{field.FieldName}請輸入{field.WordLimit}個字以內";
                        break;
                    case AnswerTypeEnum.SingleChoice:
                    case AnswerTypeEnum.DropDownList:
                        foreach (var item in field.Options)
                        {
                            if (item.Id.ToString() == data)
                            {
                                memoData = collection[data + "_memo" + item.OptionName];
                                if (string.IsNullOrEmpty(memoData) && item.MemoType == (MemoTypeEnum)2)
                                {
                                    errMsg = $"請輸入{item.OptionName}";
                                    validResultList.Add(item.Id.ToString() + "_memo" + item.OptionName, errMsg);
                                    errMsg = "";
                                }
                            }
                        }
                        break;
                    case AnswerTypeEnum.MultipleChoice:
                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            string[] dataList = data.Split(',');
                            for (int i = 0; i < dataList.Length; i++)
                            {
                                foreach (var item in field.Options)
                                {
                                    if (item.Id.ToString() == dataList[i])
                                    {
                                        memoData = memoData + (i == 0 ? "" : ",") + collection[dataList[i] + "_memo" + item.OptionName];
                                        if (string.IsNullOrEmpty(memoData) && item.MemoType == (MemoTypeEnum)2)
                                        {
                                            errMsg = $"請輸入{item.OptionName}";
                                            validResultList.Add(item.Id.ToString() + "_memo" + item.OptionName, errMsg);
                                            errMsg = "";
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case AnswerTypeEnum.Email:
                        if (data.Length > 0 && !new DataCheckUtility().IsEmail(data))
                            errMsg = $"{field.FieldName}請輸入Email格式";
                        break;
                    case AnswerTypeEnum.DateObjcet:
                        if (data.Length > 0 && !DateTime.TryParse(data, out DateTime dateTimeTemp))
                            errMsg = $"{field.FieldName}請輸入日期格式";
                        break;
                    case AnswerTypeEnum.Identity:
                        if (data.Length > 0 && !new DataCheckUtility().CheckIdCardNumber(data))
                            errMsg = $"{field.FieldName}請輸入身分證字號格式";
                        break;
                    case AnswerTypeEnum.Address:
                        break;
                    case AnswerTypeEnum.File:
                        IFormFile file = collection.Files[field.Id.ToString()]!;
                        if ((file == null || file.Length <= 0) && field.IsRequired)
                        {
                            if (!FileExistCheck(ref data, field.Id, recordId))
                            {
                                errMsg = $"請上傳{field.FieldName}";
                            }
                        }
                        else if (file != null && file.Length > 0)
                        {
                            if (FileFormatCheck(field, ref errMsg, file))
                            {
                                data = GetFileName(file);
                            }
                        }
                        break;
                }
                field.Value = data;
                field.MemoValue = memoData;
                field.Remark = collection[field.Id.ToString() + "_remark"];
                if (!string.IsNullOrWhiteSpace(errMsg))
                    validResultList.Add(field.Id.ToString(), errMsg);
            }
        }


        public void Create(DocumentFormViewModel model, string recordId, IFormFileCollection fileCollection)
        {
            foreach (var item in model.Fields)
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                    continue;

                DocumentRecord record = new DocumentRecord
                {
                    RegId = recordId,
                    DocumentId = model.Id,
                    DocumentFieldId = item.Id,
                    FilledText = item.Value,
                    MemoText = item.MemoValue,
                    Remark = item.Remark,
                };
                _unitOfWork.DocumentRepository.InsertDocumentRecord(record, recordId);

                if (item.FieldType == AnswerTypeEnum.File && !string.IsNullOrWhiteSpace(item.Value))
                {
                    string[] fileNames = item.Value.Split(';');
                    if (fileNames!= null && fileNames.Length > 0)
                    {
                        IFormFile file = fileCollection[item.Id.ToString()]!;
                        FileUpload(file, fileNames[1]);
                    }
                }
            }
        }

        #region 檔案上傳

        /// <summary>
        /// 檢查檔案已存在
        /// </summary>
        private bool FileExistCheck(ref string data, string fieldID, string recordId)
        {
            if (!string.IsNullOrEmpty(recordId))
            {
                var fileField = _unitOfWork.DocumentRepository.GetDocumentRecord(fieldID, recordId);
                if (fileField != null)
                {
                    data = fileField.FilledText!;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 檢查副檔名
        /// </summary>
        private bool FileFormatCheck(DocumentFieldViewModel field, ref string errMsg, IFormFile postedFile)
        {
            double kb = Math.Round((double)(postedFile.Length / 1024), 2);
            double mb = Math.Round(kb / 1024, 2);  //length/1024 => k k/1024 => m

            var fileExtension = Path.GetExtension(postedFile.FileName);
            if (!string.IsNullOrEmpty(field.FileExtension) && !field.FileExtension.Contains(fileExtension))
            {
                errMsg = $"檔案不允許上傳{fileExtension}格式";
                return false;
            }
            else if (field.FileSizeLimit != 0 && field.FileSizeLimit < mb)
            {
                errMsg = $"{field.FieldName}檔案太大，請上傳{ field.FileSizeLimit}MB以內的檔案";
                return false;
            }
            else
                return true;
        }
        /// <summary>
        /// 取得新檔案名稱
        /// </summary>
        public string GetFileName(IFormFile file)
        {
            if (file != null && (file.Length > 0))
            {
                string fileName = Path.GetFileName(file.FileName);
                string fileExt = Path.GetExtension(fileName);
                string newfileName = Guid.NewGuid().ToString() + fileExt;
                return fileName + ";" + newfileName;
            }
            return "";
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        public void FileUpload(IFormFile file, string fileName)
        {
            string fileFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");
            if (!Directory.Exists(fileFolder))
                Directory.CreateDirectory(fileFolder);

            if (file != null && (file.Length > 0))
            {
                var filePath = Path.Combine(fileFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }

        #endregion

        public List<DocumentRecordViewModel> GetRecordData(string regId, string documentId)
        {
            IEnumerable<DocumentRecordViewModel> data = _unitOfWork.DocumentRepository.GetDocumentRecordData(regId, documentId);

            foreach (var item in data)
            {
                switch (item.FieldType)
                {
                    case (byte)AnswerTypeEnum.Panel:
                        break;
                    case (byte)AnswerTypeEnum.SingleLine:
                    case (byte)AnswerTypeEnum.MultiLine:
                    case (byte)AnswerTypeEnum.Email:
                    case (byte)AnswerTypeEnum.Identity:
                    case (byte)AnswerTypeEnum.DateObjcet:
                    case (byte)AnswerTypeEnum.File:
                        item.Value = item.FilledText;
                        break;
                    case (byte)AnswerTypeEnum.Address:
                        item.Value = item.FilledText.Replace(";", "");
                        break;
                    case (byte)AnswerTypeEnum.DropDownList:
                    case (byte)AnswerTypeEnum.SingleChoice:
                        var option = _unitOfWork.DocumentRepository.GetFieldOption(item.FilledText, "");
                        item.Value = option == null ? "" : option.OptionName;
                        break;
                    case (byte)AnswerTypeEnum.MultipleChoice:
                        string[] optionIDList = item.FilledText.Split(',');
                        string[] memoList = item.MemoValue.Split(',');
                        List<string> optionNames = new List<string>();
                        int index = 0;
                        foreach (string optionID in optionIDList)
                        {
                            var optionForMultiple = _unitOfWork.DocumentRepository.GetFieldOption(optionID, "");
                            if (memoList.Length > 1 && !string.IsNullOrWhiteSpace(memoList[index]))
                            {
                                optionForMultiple.OptionName += " (" + memoList[index] + ")";
                            }
                            optionNames.Add(optionForMultiple == null ? "" : optionForMultiple.OptionName);
                            index++;
                        }
                        item.MemoValue = "";
                        item.Value = string.Join("、", optionNames.ToArray());
                        break;
                }
            }
            return data.ToList();
        }
    }
}
