using Microsoft.AspNetCore.Mvc;
using NewsFeedService.WebAPI.Data;
using NewsFeedService.WebAPI.Services;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeedService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsFeedController : ControllerBase
    {
        private readonly INewsFeedService _newsFeedService;

        public NewsFeedController(INewsFeedService newsFeedService)
        {
            _newsFeedService = newsFeedService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = (await _newsFeedService.Get(new[] { id }, null)).FirstOrDefault();
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] Filters filters)
        {
            var newsItems = await _newsFeedService.Get(null, filters);

            return Ok(newsItems);
        }

        [HttpPost]
        public async Task<IActionResult> Add(NewsFeedItem newsFeedItem)
        {
            await _newsFeedService.Add(newsFeedItem);
            return Ok(newsFeedItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = (await _newsFeedService.Get(new[] { id }, null)).FirstOrDefault();
            if (user == null)
                return NotFound();


            await _newsFeedService.Delete(user);
            return NoContent();
        }
    }
}
