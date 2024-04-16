using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class DocumentFieldOption
{
    public int Id { get; set; }

    public int DocumentFieldId { get; set; }

    public string OptionName { get; set; } = null!;

    public byte MemoType { get; set; }

    public int Sort { get; set; }
}
