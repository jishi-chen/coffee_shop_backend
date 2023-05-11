using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class ExamineAnswer
    {
        public int SeqNo { get; set; }
        public Guid Id { get; set; }
        public Guid ExamineQuestionId { get; set; }
        public string Caption { get; set; } = null!;
        public short Sort { get; set; }
        public byte MemoType { get; set; }
        public short Score { get; set; }
        public Guid Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? Updator { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
