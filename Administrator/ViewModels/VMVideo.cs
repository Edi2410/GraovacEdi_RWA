using DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Administrator.ViewModels
{
    public class VMVideo
    {
        public int? Id { get; set; }

        public DateTime CreatedAt { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Video name is required")]
        public string Name { get; set; } = null!;

        [DisplayName("Description")]
        public string? Description { get; set; }

        [DisplayName("Genre")]
        [Required(ErrorMessage = "Video Genre is required")]
        public int GenreId { get; set; }

        [DisplayName("Duration (s)")]
        [Required(ErrorMessage = "Video TotalSeconds is required")]
        public int TotalSeconds { get; set; }

        [DisplayName("Stream URL")]
        [Required(ErrorMessage = "Video StreamingUrl is required")]
        public string? StreamingUrl { get; set; }

        [DisplayName("Image")]
        [Required(ErrorMessage = "Video Image is required")]
        public int? ImageId { get; set; }
    }
}
