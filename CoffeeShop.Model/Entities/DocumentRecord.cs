using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class DocumentRecord
{
    public long SeqNo { get; set; }

    public int RegId { get; set; }

    public int DocumentId { get; set; }

    public int DocumentFieldId { get; set; }

    public string FilledText { get; set; } = null!;

    public string MemoText { get; set; } = null!;

    public string Remark { get; set; } = null!;
}
