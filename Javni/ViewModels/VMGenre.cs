using DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Javni.ViewModels
{
    public class VMGenre
    {
        public int Id { get; set; }

        [DisplayName("Genre Name")]
        [Required(ErrorMessage = "Genre name is required")]
        public string Name { get; set; } = null!;

        [DisplayName("Genre Description")]
        [Required(ErrorMessage = "Genre Description is required")]
        public string? Description { get; set; }

    }
}
