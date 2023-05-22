using coffee_shop_backend.Enums;
using coffee_shop_backend.Models;
using coffee_shop_backend.Utility;
using coffee_shop_backend.ViewModels;
using System.Transactions;

namespace coffee_shop_backend.Services
{
    public class QuestionnaireFrontApi
    {
        private readonly ModelContext _context;
        public QuestionnaireFrontApi(ModelContext modelContext)
        {
            this._context = modelContext;
        }

        public void FieldDataCheck(IFormCollection collection, IEnumerable<ApplicationFieldViewModel> fieldList, Dictionary<string, string> validResultList)
        {

            foreach (var field in fieldList)
            {
                string data = collection[field.ID.ToString()];
                string memoData = "";
                string errMsg = "";
                switch (field.FieldType)
                {
                    case AnswerTypeEnum.SingleLine:
                    case AnswerTypeEnum.MultiLine:
                        if (field.IsRequired && string.IsNullOrWhiteSpace(data))
                            errMsg = $"請輸入{field.FieldName}";
                        else if (field.WordLimit != 0 && data.Length > field.WordLimit)
                            errMsg = $"{field.FieldName}請輸入{field.WordLimit}個字以內";
                        break;
                    case AnswerTypeEnum.SingleChoice:
                    case AnswerTypeEnum.DropDownList:
                        if (field.IsRequired && string.IsNullOrWhiteSpace(data))
                            errMsg = $"請輸入{field.FieldName}";
                        else
                        {
                            foreach (var item in field.Options)
                            {
                                if (item.ID.ToString() == data)
                                {
                                    memoData = collection[data + "_memo" + item.OptionName];
                                    if (string.IsNullOrEmpty(memoData) && item.MemoType == (MemoTypeEnum)2)
                                    {
                                        errMsg = $"請輸入{item.OptionName}";
                                        validResultList.Add(item.ID.ToString() + "_memo" + item.OptionName, errMsg);
                                        errMsg = "";
                                    }
                                }
                            }
                        }
                        break;
                    case AnswerTypeEnum.MultipleChoice:
                        if (field.IsRequired && string.IsNullOrWhiteSpace(data))
                            errMsg = $"請輸入{field.FieldName}";
                        else if (!string.IsNullOrWhiteSpace(data))
                        {
                            string[] dataList = data.Split(',');
                            for (int i = 0; i < dataList.Length; i++)
                            {
                                foreach (var item in field.Options)
                                {
                                    if (item.ID.ToString() == dataList[i])
                                    {
                                        memoData = memoData + (i == 0 ? "" : ",") + collection[dataList[i] + "_memo" + item.OptionName];
                                        if (string.IsNullOrEmpty(memoData) && item.MemoType == (MemoTypeEnum)2)
                                        {
                                            errMsg = $"請輸入{item.OptionName}";
                                            validResultList.Add(item.ID.ToString() + "_memo" + item.OptionName, errMsg);
                                            errMsg = "";
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case AnswerTypeEnum.Email:
                        if (field.IsRequired && string.IsNullOrWhiteSpace(data))
                            errMsg = $"請輸入{field.FieldName}";
                        else if (data.Length > 0 && !new DataCheckUtility().IsEmail(data))
                            errMsg = $"{field.FieldName}請輸入Email格式";
                        break;
                    case AnswerTypeEnum.DateObjcet:
                        if (field.IsRequired && string.IsNullOrWhiteSpace(data))
                            errMsg = $"請輸入{field.FieldName}";
                        else if (data.Length > 0 && !DateTime.TryParse(data, out DateTime dateTimeTemp))
                            errMsg = $"{field.FieldName}請輸入日期格式";
                        break;
                }
                field.Value = data;
                field.MemoValue = memoData;

                if (!string.IsNullOrWhiteSpace(errMsg))
                    validResultList.Add(field.ID.ToString(), errMsg);
            }
        }


        public void Create(QuestionDetailViewModel viewModel)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Reg reg = new Reg()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test"
                    };
                    _context.Regs.Add(reg);

                    CreateFieldData(viewModel, reg.Id);
                    _context.SaveChanges();
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {

            }
        }

        public void CreateFieldData(QuestionDetailViewModel viewModel, Guid applicationRecordID)
        {
            string fileFolder = "";

            foreach (var item in viewModel.Questions)
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                    continue;

                ApplicationRecord model = new ApplicationRecord
                {
                    RegId = applicationRecordID,
                    ApplicationId = viewModel.ID,
                    ApplicationFieldId = new Guid(item.ID),
                    FilledText = item.Value,
                    AnswerText = item.MemoValue,
                    Remark = "",
                };
                _context.ApplicationRecords.Add(model);

                if (item.FieldType == AnswerTypeEnum.File && !string.IsNullOrWhiteSpace(item.Value))
                {
                    string[] fileNames = item.Value.Split(';');
                    string newFileName = fileNames[1];

                    string tempFilePath = Path.Combine($"{fileFolder}/Temp", newFileName);
                    if (File.Exists(tempFilePath))
                        File.Move(tempFilePath, Path.Combine(fileFolder, newFileName));
                }
            }
            _context.SaveChanges();
        }


        public List<RecordViewModel> GetData(string regId, string applicationId)
        {
            IEnumerable<RecordViewModel> data = from field in _context.ApplicationFields
                                                join detail in
                                                (from a in _context.ApplicationRecords
                                                where a.RegId.ToString() == regId
                                                 select (a))
                                                on field.Id equals detail.ApplicationFieldId into ps
                                                from detail in ps.DefaultIfEmpty()
                                                where field.ApplicationId.ToString() == applicationId
                                                select new RecordViewModel
                                                {
                                                    FieldID = field.Id,
                                                    FieldName = field.FieldName,
                                                    Sort = field.Sort,
                                                    FilledText = detail == null ? "" : detail.FilledText,
                                                    FieldType = field.FieldType,
                                                    IsRequired = field.IsRequired,
                                                    IsFixed = field.IsFixed,
                                                    MemoValue = detail == null ? "" : detail.AnswerText,
                                                    Remark = detail == null ? "" : detail.Remark,
                                                };
            data = data.OrderBy(x => x.Sort).ToList();
            foreach (var item in data)
            {
                switch (item.FieldType)
                {
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
                        var option = _context.ApplicationFieldOptions.FirstOrDefault(x => x.Id.ToString() == item.FilledText);
                        item.Value = option == null ? "" : option.OptionName;
                        break;
                    case (byte)AnswerTypeEnum.MultipleChoice:
                        string[] optionIDList = item.FilledText.Split(',');
                        string[] memoList = item.MemoValue.Split(',');
                        List<string> optionNames = new List<string>();
                        int index = 0;
                        foreach (string optionID in optionIDList)
                        {
                            var optionForMultiple = _context.ApplicationFieldOptions.FirstOrDefault(x => x.Id.ToString() == optionID);
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
