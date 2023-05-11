using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class ExamineQuestion
    {
        public int SeqNo { get; set; }
        public Guid Id { get; set; }
        public Guid ExamineId { get; set; }
        public Guid? ParentId { get; set; }
        public string Caption { get; set; } = null!;
        public byte AnswerType { get; set; }
        public bool IsRequired { get; set; }
        public short Sort { get; set; }
        public Guid Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? Updator { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
