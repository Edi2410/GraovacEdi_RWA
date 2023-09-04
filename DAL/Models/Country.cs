
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Country
{
    public int Id { get; set; }

    [DisplayName("Country Code")]
    [Required(ErrorMessage = "Code is required")]
    public string Code { get; set; } = null!;

    [DisplayName("Country Name")]
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
