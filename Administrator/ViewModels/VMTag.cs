using DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Administrator.ViewModels
{
    public class VMTag
    {
        public int Id { get; set; }

        [DisplayName("Tag Name")]
        [Required(ErrorMessage = "Tag name is required")]
        public string Name { get; set; } = null!;

    }
}
