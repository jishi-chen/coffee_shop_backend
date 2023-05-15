using System.ComponentModel;

namespace coffee_shop_backend.Enums
{
    public enum MemoTypeEnum
    {
        [Description("不須備註")]
        None = 0,
        [Description("一般備註")]
        Normal = 1,
        [Description("必填備註")]
        Required = 2
    }

    public enum AnswerTypeEnum
    {
        [Description("分類項目")]
        Panel = 0,
        [Description("單選")]
        SingleSelect = 1,
        [Description("複選")]
        MultiSelect = 2,
        [Description("單行文字")]
        SingleLine = 3,
        [Description("多行文字")]
        MultiLine = 4,
        [Description("日期物件")]
        Date = 5,
        [Description("Email")]
        Email = 6,
        [Description("地址")]
        Address = 7,
        [Description("檔案上傳")]
        File = 8,
    }
}
