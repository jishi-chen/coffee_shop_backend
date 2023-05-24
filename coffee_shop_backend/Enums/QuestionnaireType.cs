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
        [Description("單行文字")]
        SingleLine = 0,
        [Description("多行文字")]
        MultiLine = 1,
        [Description("單選題")]
        SingleChoice = 2,
        [Description("複選題")]
        MultipleChoice = 3,
        [Description("下拉選單")]
        DropDownList = 4,
        [Description("電子信箱")]
        Email = 5,
        [Description("日期物件")]
        DateObjcet = 6,
        [Description("地址")]
        Address = 7,
        [Description("檔案上傳")]
        File = 8,
        [Description("身份證字號")]
        Identity = 9,
        [Description("父題目")]
        Panel = 10
    }
}
