using Administrator.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Administrator.ViewModels
{
    public class VideosVM
    {
        public List<VideoResponse> Videos { get; set; }
        public SelectList Genres { get; set; } = null!;
        public string? GenreFilter { get; set; }
        public string? NameFilter { get; set; }
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public int PageCount { get; set; }

    }

}
