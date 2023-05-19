using coffee_shop_backend.Enums;
using coffee_shop_backend.Models;

namespace coffee_shop_backend.ViewModels
{
    public class QuestionDetailViewModel
    {
        /// <summary> 問卷代碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 標題 </summary>
        public string Caption { get; set; } = string.Empty;

        /// <summary> 頁首文字 </summary>
        public string HeadText { get; set; } = string.Empty;

        /// <summary> 頁尾文字 </summary>
        public string FooterText { get; set; } = string.Empty;

        /// <summary> 問題集 </summary>
        public List<ApplicationFieldViewModel> Questions { get; set; } = new List<ApplicationFieldViewModel>();
        public Dictionary<string, string> ValidResults { get; set; } = new Dictionary<string, string>();
    }

    public class ApplicationFieldViewModel
    {

        public string ID { get; set; }

        public string FieldName { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public AnswerTypeEnum FieldType { get; set; }

        public short WordLimit { get; set; }

        public short RowLimit { get; set; }

        public short FileSizeLimit { get; set; }
        public int Sort { get; set; }

        public bool IsRequired { get; set; }
        public bool IsFixed { get; set; }

        public List<ApplicationFieldOptionViewModel> Options { get; set; } = new List<ApplicationFieldOptionViewModel>();

        public string Value { get; set; } = string.Empty;
        public string MemoValue { get; set; } = string.Empty;

        public string ErrMsg { get; set; } = string.Empty;
    }



    /// <summary> 問卷選項題目 </summary>
    public class ApplicationFieldOptionViewModel
    {
        public string ID { get; set; } = string.Empty;

        /// <summary> 標題 </summary>
        public string OptionName { get; set; } = string.Empty;

        /// <summary> 排序值 </summary>
        public int Sort { get; set; }

        /// <summary> 備註種類 </summary>
        public MemoTypeEnum MemoType { get; set; }

        /// <summary> 是否已選取 </summary>
        public bool Checked { get; set; }

        /// <summary> 輸入的備註 </summary>
        public string MemoValue { get; set; } = string.Empty;
    }
}
