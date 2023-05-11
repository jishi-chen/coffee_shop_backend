using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class ExamineFillInRecordDetail
    {
        public int SeqNo { get; set; }
        public Guid ExamineFillInRecordsId { get; set; }
        public Guid ExamineQuestionId { get; set; }
        public Guid? ExamineAnswerId { get; set; }
        public string? AnswerText { get; set; }
    }
}
