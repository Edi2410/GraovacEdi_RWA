using AutoMapper;
using CoreWebApi.Models;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreWebApi.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly RwaMoviesContext _context;
        private readonly IMapper _mapper;

        public VideosController(RwaMoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Videos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoDTO>>> GetVideos()
        {
            if (_context.Videos == null)
            {
                return NotFound();
            }
            var videos = await _context.Videos
                        .Include(v => v.Genre)
                        .ToListAsync();
                        //.Include(v => v.VideoTags).ThenInclude(vt => vt.Tag)
            return _mapper.Map<List<VideoDTO>>(videos);
        }

        // GET: api/Videos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoDTO>> GetVideo(int id)
        {
            if (_context.Videos == null)
            {
                return NotFound();
            }
            var video = await _context.Videos.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            return _mapper.Map<VideoDTO>(video);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<VideoDTO>>> Search(int page, int size, string? orderBy, string? direction, string? filter)
        {
            var videos = await _context.Videos.ToListAsync();
            IEnumerable<Video> ordered;


            var filteredVideos =
                videos.Where(x =>
                    x.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase));

            // Ordering
            if (string.Compare(orderBy, "id", true) == 0)
            {
                ordered = filteredVideos.OrderBy(x => x.Id);
            }
            else if (string.Compare(orderBy, "name", true) == 0)
            {
                ordered = filteredVideos.OrderBy(x => x.Name);
            }
            else if (string.Compare(orderBy, "totalSeconds", true) == 0)
            {
                ordered = filteredVideos.OrderBy(x => x.TotalSeconds);
            }
            else
            {
                // default: order by Id
                ordered = filteredVideos.OrderBy(x => x.Id);
            }

            // For descending order we just reverse it
            if (string.Compare(direction, "desc", true) == 0)
            {
                ordered = ordered.Reverse();
            }


            // Now we can page the correctly ordered items
            var retVal = ordered.Skip((page - 1) * size).Take(size);


            return Ok(_mapper.Map<List<VideoDTO>>(retVal));
        }

        // PUT: api/Videos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideo(int id, VideoDTO videoDTO)
        {
            Video video = _mapper.Map<Video>(videoDTO);
            if (id != video.Id)
            {
                return BadRequest();
            }

            _context.Entry(video).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Videos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VideoDTO>> PostVideo(VideoDTO video)
        {
            if (_context.Videos == null)
            {
                return Problem("Entity set 'RwaMoviesContext.Videos'  is null.");
            }

            _context.Videos.Add(_mapper.Map<Video>(video));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideo", new { id = video.Id }, video);
        }

        // DELETE: api/Videos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            if (_context.Videos == null)
            {
                return NotFound();
            }
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoExists(int id)
        {
            return (_context.Videos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
