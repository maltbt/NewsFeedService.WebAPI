using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NewsFeedService.WebAPI.Data;
using NewsFeedService.WebAPI.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace NewsFeedService.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NewsFeedController : ControllerBase
    {

        // Ideally should be externally configured (SizeLimit, Expiry, etc) and then dependency injected,
        // but the instructions for this exercise are to change only the controller class
        private static readonly IMemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly INewsFeedService _newsFeedService;


        public NewsFeedController(INewsFeedService newsFeedService)
        {
            _newsFeedService = newsFeedService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {


            
            /*
            if (MemoryCache.Get("NewsItems") != null)
            {
                MemoryCache.Remove("NewsItems");
            }
            */
            


            var user = (await _newsFeedService.Get(new[] { id }, null)).FirstOrDefault();
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] Filters filters)
        {
            
            var cacheKey = new FiltersCacheKey(filters);
            
            var newsItems = Cache.Get<IEnumerable<NewsFeedItem>>(cacheKey);
            if (newsItems == null)
            {
                newsItems = await _newsFeedService.Get(null, filters);
                Cache.Set(cacheKey, newsItems, new CancellationChangeToken(_cancellationTokenSource.Token));
                
            }

            return Ok(newsItems);
            
        }

        [HttpPost]
        public async Task<IActionResult> Add(NewsFeedItem newsFeedItem)
        {
            /*if (_cancellationTokenSource != null && _cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            _cancellationTokenSource = new CancellationTokenSource();*/

            ResetCache();

            await _newsFeedService.Add(newsFeedItem);
            return Ok(newsFeedItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            ResetCache();

            var user = (await _newsFeedService.Get(new[] { id }, null)).FirstOrDefault();
            if (user == null)
                return NotFound();


            await _newsFeedService.Delete(user);
            return NoContent();
        }

        private static void ResetCache()
        {
            if (_cancellationTokenSource != null && _cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }

            _cancellationTokenSource = new CancellationTokenSource();
        }

        // Ideally should be declared in its own file but the instructions for this exercise are to change only the controller class
        //private class FiltersCacheKey : Filters
        public class FiltersCacheKey : Filters
        {
            public FiltersCacheKey(Filters filters)
            {
                Body = filters.Body;
                AuthorNames = filters.AuthorNames;
                Title = filters.Title;
            }

            protected bool Equals(FiltersCacheKey other)
            {
               
                return Equals(Body, other.Body) && Equals(AuthorNames, other.AuthorNames) && Equals(Title, other.Title);
            }

            private bool Equals(string[] array1, string[] array2)
            {
                return array1 == array2 || array1.SequenceEqual(array2);
            }
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((FiltersCacheKey)obj);
            }

            public override int GetHashCode()
            {

                return HashCode.Combine(GetHashCode(Body), GetHashCode(AuthorNames), GetHashCode(Title));
            }

            private int GetHashCode(string[] array)
            {
                if (array == null)
                {
                    return 0;
                }

                int hashCode = 0;

                foreach (var element in array)
                {
                    if (element != null)
                    {
                        hashCode += 13 * element.GetHashCode();
                    }
                }

                return hashCode;
            }
        }
    }


}
