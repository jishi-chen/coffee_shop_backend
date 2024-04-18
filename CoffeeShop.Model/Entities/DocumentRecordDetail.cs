using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class DocumentRecordDetail
{
    public long SeqNo { get; set; }

    public int DocumentRecordId { get; set; }

    public int DocumentFieldId { get; set; }

    public string? FilledText { get; set; }

    public string? MemoText { get; set; }

    public string? Remark { get; set; }
}
