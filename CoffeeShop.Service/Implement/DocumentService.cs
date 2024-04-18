using AutoMapper;
using CoffeeShop.Model.DTO;
using CoffeeShop.Model.Entities;
using CoffeeShop.Model.Enum;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using CoffeeShop.Utility.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Service.Implement
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Document> GetAdminList()
        {
            var model =  _unitOfWork.DocumentRepository.GetAdminList(null);
            _unitOfWork.Dispose();
            return model;
        }

        public DocumentViewModel GetFormData(string id)
        {
            DocumentViewModel model = new DocumentViewModel();
            model.InfoPage.StartDate = DateTime.Now;
            model.InfoPage.EndDate = DateTime.Now;
            if (!string.IsNullOrEmpty(id))
            {
                Document document = _unitOfWork.DocumentRepository.GetDocument(id);
                var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Document, DocumentDTO>());
                var mapper = config.CreateMapper();
                model.InfoPage = mapper.Map<DocumentDTO>(document);
                List<DocumentField> fields = _unitOfWork.DocumentRepository.GetFieldList(id).ToList();
                var config2 = new MapperConfiguration(cfg =>
                cfg.CreateMap<DocumentField, DocumentFieldDTO>());
                var mapper2 = config2.CreateMapper();
                model.QuestionPage.FieldList = mapper2.Map<List<DocumentFieldDTO>>(fields);
                var parentList = model.QuestionPage.FieldList.Where(x => x.FieldType == (int)AnswerTypeEnum.Panel).Select(x => new SelectListItem
                {
                    Text = x.FieldName,
                    Value = x.Id,
                }).ToList();
                model.QuestionPage.ParentFieldList.Add(new SelectListItem { Text = "---請選擇---", Value = string.Empty });
                model.QuestionPage.ParentFieldList.AddRange(parentList);
                if (_unitOfWork.DocumentRepository.GetDocumentRecordList(id).Any())
                    model.QuestionPage.HasData = true;
                _unitOfWork.Dispose();
            }
            GetAnswerTypeSettings(model.QuestionPage.AnswerTypeList);
            GetMemoTypeSettings(model.QuestionPage.MemoTypeList);
            return model;
        }
        public void InsertDocument(DocumentDTO model)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<DocumentDTO , Document>());
            var mapper = config.CreateMapper();
            var result = mapper.Map<Document>(model);
            _unitOfWork.DocumentRepository.InsertDocument(result);
            _unitOfWork.Complete();
        }
        public void UpdateDocument(DocumentDTO model)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<DocumentDTO, Document>());
            var mapper = config.CreateMapper();
            var result = mapper.Map<Document>(model);
            _unitOfWork.DocumentRepository.UpdateDocument(result);
            _unitOfWork.Complete();
        }

        public void FieldDataCheck(IFormCollection collection, IEnumerable<DocumentFieldViewModel> fieldList, Dictionary<string, string> validResultList, string recordId)
        {

            foreach (var field in fieldList)
            {
                string data = collection[field.Id];
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
                            if (item.Id == data)
                            {
                                memoData = collection[data + "_memo" + item.OptionName];
                                if (string.IsNullOrEmpty(memoData) && item.MemoType == (MemoTypeEnum)2)
                                {
                                    errMsg = $"請輸入{item.OptionName}";
                                    validResultList.Add(item.Id + "_memo" + item.OptionName, errMsg);
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
                                    if (item.Id == dataList[i])
                                    {
                                        memoData = memoData + (i == 0 ? "" : ",") + collection[dataList[i] + "_memo" + item.OptionName];
                                        if (string.IsNullOrEmpty(memoData) && item.MemoType == (MemoTypeEnum)2)
                                        {
                                            errMsg = $"請輸入{item.OptionName}";
                                            validResultList.Add(item.Id + "_memo" + item.OptionName, errMsg);
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
                        IFormFile file = collection.Files[field.Id]!;
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
                field.Remark = collection[field.Id + "_remark"];
                if (!string.IsNullOrWhiteSpace(errMsg))
                    validResultList.Add(field.Id, errMsg);
            }
        }


        public void Create(DocumentFormViewModel model, string recordId, IFormFileCollection fileCollection, bool isEdit)
        {
            List<DocumentRecordDetail> recordList = new List<DocumentRecordDetail>();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<DocumentRecordDetailDTO, DocumentRecordDetail>());
            var mapper = config.CreateMapper();

            if (!isEdit)
            {
                DocumentRecord documentRecord = new DocumentRecord()
                {
                    DocumentId = int.Parse(model.Id),
                    Name = "Admin"
                };
                recordId = _unitOfWork.DocumentRepository.InsertDocumentRecord(documentRecord).ToString();
            }
            foreach (var item in model.Fields)
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                    continue;

                DocumentRecordDetailDTO documentRecordDetailDTO = new DocumentRecordDetailDTO()
                {
                    DocumentRecordId = recordId,
                    DocumentFieldId = item.Id,
                    FilledText = item.Value,
                    MemoText = item.MemoValue,
                    Remark = item.Remark,
                };
                DocumentRecordDetail record = mapper.Map<DocumentRecordDetail>(documentRecordDetailDTO);
                recordList.Add(record);

                if (item.FieldType == AnswerTypeEnum.File && !string.IsNullOrWhiteSpace(item.Value))
                {
                    string[] fileNames = item.Value.Split(';');
                    if (fileNames != null && fileNames.Length > 0)
                    {
                        //刪除舊檔
                        string oldFileName = "";
                        IFormFile file = fileCollection[item.Id]!;
                        if (file != null && isEdit)
                        {
                            if (FileExistCheck(ref oldFileName, item.Id, recordId))
                            {
                                string[] oldFileNames = oldFileName.Split(';');
                                string fileFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", oldFileNames[1]);
                                if (File.Exists(fileFolder))
                                {
                                    File.Delete(fileFolder);
                                }
                            }
                        }
                        //新增檔案
                        FileUpload(file, fileNames[1]);
                    }
                }
            }
            if (!isEdit)
                _unitOfWork.DocumentRepository.InsertDocumentRecordDetail(recordList);
            else
                _unitOfWork.DocumentRepository.UpdateDocumentRecordDetail(recordList);
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
                errMsg = $"{field.FieldName}檔案太大，請上傳{field.FileSizeLimit}MB以內的檔案";
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
            List<DocumentRecordViewModel> data = _unitOfWork.DocumentRepository.GetDocumentRecordData(regId, documentId).ToList();

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
            _unitOfWork.Dispose();
            return data;
        }

        public List<DocumentRecordListViewModel> GetRecodList()
        {
            List<DocumentRecordListViewModel> model = _unitOfWork.DocumentRepository.GetDocumentRecordList("").ToList();
            _unitOfWork.Dispose();
            return model;
        }

        public void InsertField(DocumentViewModel model, DocumentQuestionPage QuestionPage)
        {
            var Question = QuestionPage;
            DocumentField documentField = _unitOfWork.DocumentRepository.GetDocumentField(model.QuestionPage.DocumentFieldId!);
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<DocumentField, DocumentFieldDTO>());
            var config2 = new MapperConfiguration(cfg =>
                cfg.CreateMap<DocumentFieldDTO, DocumentField>());

            var mapper = config.CreateMapper();
            var field = mapper.Map<DocumentFieldDTO>(documentField);
            var oldParentId = field.ParentId!;
            field.Id = model.QuestionPage.DocumentFieldId!;
            field.ParentId = Question.ParentId;
            field.DocumentId = model.InfoPage.Id!;
            field.FieldName = Question.Caption;
            field.FieldType = (byte)Question.AnswerType;
            field.IsRequired = Question.IsRequired;
            field.IsIncludedExport = Question.IsIncludedExport;
            field.IsEditable = Question.IsEditable;
            field.WordLimit = Question.WordLimit;
            field.FileSizeLimit = Question.FileSizeLimit;
            field.FileExtension = Question.FileExtension;
            field.Note = Question.Note;
            var list = model.QuestionPage.FieldList.Where(x => x.ParentId == Question.ParentId).ToList();
            //新增
            if (string.IsNullOrEmpty(Question.DocumentFieldId))
            {
                field.Creator = "Admin";
                field.CreateDate = DateTime.Now;
                if (list.Any())
                    field.Sort = list.OrderByDescending(x => x.Sort).FirstOrDefault()!.Sort + 1;
                else
                    field.Sort = 0;
                mapper = config2.CreateMapper();
                documentField = mapper.Map<DocumentField>(field);
                documentField.Id = _unitOfWork.DocumentRepository.InsertDocumentField(documentField);
            }
            //編輯
            else
            {
                field.Updator = "Admin";
                field.UpdateDate = DateTime.Now;
                //更換父題目要更改排序值
                if (field.ParentId != model.QuestionPage.FieldList.FirstOrDefault(x => x.Id  == field.Id)!.ParentId)
                {
                    if (list.Any())
                        field.Sort = list.OrderByDescending(x => x.Sort).FirstOrDefault()!.Sort + 1;
                    else
                        field.Sort = 0;
                }
                mapper = config2.CreateMapper();
                documentField = mapper.Map<DocumentField>(field);
                _unitOfWork.DocumentRepository.UpdateDocumentField(documentField);
                ResetFieldSort(field.DocumentId, oldParentId);
            }
            if (field.FieldType == (int)AnswerTypeEnum.SingleChoice || field.FieldType == (int)AnswerTypeEnum.MultipleChoice)
            {
                _unitOfWork.DocumentRepository.DeleteFieldOptions(documentField.Id.ToString());
                if (model.QuestionPage.OptionList.Any())
                {
                    foreach (var item in model.QuestionPage.OptionList)
                    {
                        DocumentFieldOption option = new DocumentFieldOption()
                        {
                            DocumentFieldId = documentField.Id,
                            OptionName = item.Text,
                            Sort = (short)(item.Sort.HasValue ? item.Sort.Value : 0),
                            MemoType = (byte)item.MemoType,
                        };
                        _unitOfWork.DocumentRepository.InsertFieldOption(option);
                    }
                }
            }
            _unitOfWork.Complete();
        }
        public void EditField(DocumentViewModel model, string fieldId)
        {
            DocumentField documentField = _unitOfWork.DocumentRepository.GetDocumentField(fieldId);
            if (documentField != null)
            {
                var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<DocumentField, DocumentFieldDTO>());
                var mapper = config.CreateMapper();

                DocumentFieldDTO field = mapper.Map<DocumentFieldDTO>(documentField);
                model.QuestionPage.ParentId = field.ParentId;
                model.QuestionPage.DocumentFieldId = field.Id;
                model.QuestionPage.Caption = field.FieldName;
                model.QuestionPage.WordLimit = field.WordLimit;
                model.QuestionPage.FileSizeLimit = field.FileSizeLimit;
                model.QuestionPage.FileExtension = field.FileExtension;
                model.QuestionPage.IsIncludedExport = field.IsIncludedExport;
                model.QuestionPage.IsRequired = field.IsRequired;
                model.QuestionPage.IsEditable = field.IsEditable;
                model.QuestionPage.Note = field.Note;
                model.QuestionPage.AnswerType = field.FieldType;
                model.QuestionPage.OptionList.Clear();
                if (model.QuestionPage.AnswerType == (int)AnswerTypeEnum.SingleChoice || model.QuestionPage.AnswerType == (int)AnswerTypeEnum.MultipleChoice)
                {
                    IEnumerable<DocumentFieldOption> optionList = _unitOfWork.DocumentRepository.GetFieldOption(fieldId);
                    if (optionList.Any())
                    {
                        foreach (var item in optionList)
                        {
                            model.QuestionPage.OptionList.Add(new AnswerOption()
                            {
                                Text = item.OptionName,
                                MemoType = item.MemoType,
                                Sort = item.Sort,
                                MemoText = EnumHelper.GetDescription((MemoTypeEnum)item.MemoType),
                            });
                        }
                    }
                }
            }
        }
        public DocumentFormViewModel GetFrontFormData(string id, string recordId)
        {
            Document document = _unitOfWork.DocumentRepository.GetDocument(id);
            IEnumerable<DocumentField> fields = _unitOfWork.DocumentRepository.GetFieldList(id).ToList();
            IEnumerable<DocumentRecordDetail> record = _unitOfWork.DocumentRepository.GetDocumentRecord(recordId);
            bool isEdit = record.Count() > 0;
            DocumentFormViewModel model = new DocumentFormViewModel()
            {
                Id = document.Id.ToString(),
                RecordId = recordId,
                Caption = document.Caption,
                HeadText = document.HeadText,
                FooterText = document.FooterText,
            };
            foreach (DocumentField field in fields)
            {
                DocumentFieldViewModel dfvm = new DocumentFieldViewModel()
                {
                    Id = field.Id.ToString(),
                    ParentId = field.ParentId.HasValue ? field.ParentId.Value.ToString() : null,
                    FieldName = field.FieldName,
                    Note = field.Note,
                    FieldType = (AnswerTypeEnum)field.FieldType,
                    WordLimit = field.WordLimit,
                    RowLimit = field.RowLimit,
                    FileSizeLimit = field.FileSizeLimit,
                    FileExtension = field.FileExtension,
                    Sort = field.Sort,
                    IsRequired = field.IsRequired,
                    IsIncludedExport = field.IsIncludedExport,
                    IsEditable = field.IsEditable,
                };
                if (field.FieldType == (int)AnswerTypeEnum.SingleChoice || field.FieldType == (int)AnswerTypeEnum.MultipleChoice || field.FieldType == (int)AnswerTypeEnum.DropDownList)
                {
                    IEnumerable<DocumentFieldOption> options = _unitOfWork.DocumentRepository.GetFieldOption(field.Id.ToString()).ToList();
                    foreach (DocumentFieldOption option in options)
                    {
                        DocumentFieldOptionViewModel dfovm = new DocumentFieldOptionViewModel()
                        {
                            Id = option.Id.ToString(),
                            OptionName = option.OptionName,
                            Sort = option.Sort,
                            MemoType = (MemoTypeEnum)option.MemoType,
                        };
                        dfvm.Options.Add(dfovm);
                    }
                }
                //修改
                if (isEdit)
                {
                    var doc = record.FirstOrDefault(x => x.DocumentFieldId == field.Id);
                    if (doc != null)
                    {
                        dfvm.Value = doc.FilledText;
                        dfvm.MemoValue = doc.MemoText;
                        dfvm.Remark = doc.Remark;
                        if (dfvm.Options.Count() > 0 && !string.IsNullOrEmpty(dfvm.MemoValue) && !string.IsNullOrEmpty(dfvm.Value))
                        {
                            string[] optionId = dfvm.Value.Split(',');
                            string[] memoValue = dfvm.MemoValue.Split(',');
                            for (int i = 0; i < memoValue.Count(); i++)
                            {
                                dfvm.Options.FirstOrDefault(x => x.Id == optionId[i])!.MemoValue = memoValue[i];
                            }
                        }
                    }
                }
                model.Fields.Add(dfvm);
            }
            _unitOfWork.Dispose();
            return model;
        }

        public int DeleteField(string fieldId)
        {
            DocumentField field = _unitOfWork.DocumentRepository.GetDocumentField(fieldId);
            _unitOfWork.DocumentRepository.DeleteField(fieldId);
            _unitOfWork.DocumentRepository.DeleteFieldOptions(fieldId);
            ResetFieldSort(field.DocumentId.ToString(), field.ParentId.HasValue ? field.ParentId.Value.ToString() : null);
            _unitOfWork.Complete();
            return field.DocumentId;
        }

        public void GetAnswerTypeSettings(List<SelectListItem> list)
        {
            foreach (var item in Enum.GetValues(typeof(AnswerTypeEnum)))
            {
                list.Add(new SelectListItem()
                {
                    Text = EnumHelper.GetDescription((AnswerTypeEnum)item),
                    Value = ((int)item).ToString(),
                });
            }
        }
        public void GetMemoTypeSettings(List<SelectListItem> list)
        {
            foreach (var item in Enum.GetValues(typeof(MemoTypeEnum)))
            {
                list.Add(new SelectListItem()
                {
                    Text = EnumHelper.GetDescription((MemoTypeEnum)item),
                    Value = ((int)item).ToString(),
                });
            }
        }

        public void ResetFieldSort(string documentId, string parentId)
        {
            IEnumerable<DocumentField> fields = _unitOfWork.DocumentRepository.GetFieldList(documentId, parentId);
            int q = 0;
            foreach (var item in fields)
            {
                item.Sort = q;
                q++;
            }
            _unitOfWork.DocumentRepository.UpdateFieldSort(fields);
        }
        public void SetFieldSort(DocumentViewModel model, string fieldId, bool direction)
        {
            DocumentField field = _unitOfWork.DocumentRepository.GetDocumentField(fieldId);
            if (field != null)
            {
                int d = direction ? -1 : 1;
                field.Sort += d;
                DocumentField changedField = _unitOfWork.DocumentRepository.GetDocumentField(field.DocumentId.ToString(), field.ParentId.HasValue ? field.ParentId.Value.ToString() : null, field.Sort)!;
                changedField.Sort -= d;
                List<DocumentField> updateFields = new List<DocumentField>();
                updateFields.Add(field);
                updateFields.Add(changedField);
                _unitOfWork.DocumentRepository.UpdateFieldSort(updateFields);
            }
            _unitOfWork.Complete();
        }
    }
}
