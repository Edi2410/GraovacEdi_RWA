using System;
using System.Collections.Generic;

namespace CoreWebApi.Models;

public partial class TagDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
