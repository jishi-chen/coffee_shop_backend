using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class DocumentRecord
{
    public int Id { get; set; }

    public int DocumentId { get; set; }

    public string Name { get; set; } = null!;
}
