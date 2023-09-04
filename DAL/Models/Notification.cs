using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Notification
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required(ErrorMessage = "ReceiverEmail is required")]
    [EmailAddress]
    public string ReceiverEmail { get; set; } = null!;

    public string? Subject { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public string Body { get; set; } = null!;

    public DateTime? SentAt { get; set; }
}
