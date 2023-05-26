using coffee_shop_backend.Enums;
using coffee_shop_backend.Interface;
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

        public void FieldDataCheck(IFormCollection collection, IEnumerable<DocumentFieldViewModel> fieldList, Dictionary<string, string> validResultList)
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
                        if (field.IsRequired)
                        {
                            if (file == null || file.Length <= 0)
                            {
                                //if (!FileCheckExists(ref data, field.ID, recordID))
                                //{
                                //    errMsg = $"請上傳{field.FieldName}";
                                //}
                            }
                            //else if (file != null && file.Length > 0)
                                //FileFormatCheck(fileFormat, field, ref data, ref errMsg, postedFile);
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


        public void Create(QuestionDetailViewModel viewModel)
        {
            try
            {
                
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

                if (item.FieldType == AnswerTypeEnum.File && !string.IsNullOrWhiteSpace(item.Value))
                {
                    string[] fileNames = item.Value.Split(';');
                    string newFileName = fileNames[1];

                    string tempFilePath = Path.Combine($"{fileFolder}/Temp", newFileName);
                    if (File.Exists(tempFilePath))
                        File.Move(tempFilePath, Path.Combine(fileFolder, newFileName));
                }
            }
        }


        //public List<RecordViewModel> GetData(string regId, string applicationId)
        //{

           
        //}
    }
}
