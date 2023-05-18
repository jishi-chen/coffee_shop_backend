using coffee_shop_backend.Enums;

namespace coffee_shop_backend.ViewModels
{
    /// <summary> 問卷內頁 ViewModel </summary>
    public class QuestionDetailViewModel
    {
        /// <summary> 問卷代碼 </summary>
        public Guid ID { get; set; }

        /// <summary> 標題 </summary>
        public string Caption { get; set; }

        /// <summary> 頁首文字 </summary>
        public string HeadText { get; set; }

        /// <summary> 頁尾文字 </summary>
        public string FooterText { get; set; }

        /// <summary> 開始時間 </summary>
        public DateTime StartDate { get; set; }

        /// <summary> 結束時間 </summary>
        public DateTime? EndDate { get; set; }

        /// <summary> 問題集 </summary>
        public List<ExamineQuestionViewModel> Questions { get; set; } = new List<ExamineQuestionViewModel>();
    }

    /// <summary> 問卷問題基礎型別 </summary>
    public class ExamineQuestionViewModel
    {
        public string ID { get; set; }

        /// <summary> 問題種類 </summary>
        public AnswerTypeEnum AnswerType { get; set; }

        /// <summary> 排序值 </summary>
        public int Sort { get; set; }

        /// <summary> 問題 </summary>
        public string Caption { get; set; }

        /// <summary> 是否為必填 </summary>
        public bool IsRequired { get; set; }

        /// <summary> 輸入值 </summary>
        public string Value { get; set; }

        /// <summary> 子題目 </summary>
        public List<ExamineQuestionViewModel> SubQuestions { get; set; } = new List<ExamineQuestionViewModel>();

        /// <summary> 父題目 </summary>
        public ExamineQuestionViewModel ParentQuestion { get; set; } = null;

        /// <summary> 要呈現的錯誤訊息 </summary>
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }

    /// <summary> 具選項的問卷問題 </summary>
    public class ExamineSelectQuestionViewModel : ExamineQuestionViewModel
    {
        public List<ExamineAnswerViewModel> Answers { get; set; } = new List<ExamineAnswerViewModel>();
    }

    /// <summary> 備註的種類 </summary>
    public enum MomoTypeEnum
    {
        /// <summary> 無備註 </summary>
        None = 0,

        /// <summary> 一般備註 </summary>
        Normal = 1,

        /// <summary> 必填備註 </summary>
        Required = 2,
    }

    /// <summary> 問卷選項題目 </summary>
    public class ExamineAnswerViewModel
    {
        public string ID { get; set; }

        /// <summary> 標題 </summary>
        public string Caption { get; set; }

        /// <summary> 排序值 </summary>
        public int Sort { get; set; }

        /// <summary> 備註種類 </summary>
        public MomoTypeEnum MemoType { get; set; }

        /// <summary> 是否已選取 </summary>
        public bool Checked { get; set; }

        /// <summary> 輸入的備註 </summary>
        public string MemoValue { get; set; }
    }
}
