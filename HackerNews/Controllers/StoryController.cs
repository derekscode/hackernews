using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HackerNews.Models;
using HackerNews.Services;

namespace HackerNews.Controllers
{
    public class StoryController : Controller
    {
        IHackerNewsService _service;
        private IMemoryCache _cache;

        public StoryController(IHackerNewsService service, IMemoryCache cache)
        {
            _service = service;
            _cache = cache;
        }

        public IActionResult Index(string searchString)
        {
            IEnumerable<Story> stories = new List<Story>();
            var cachedStories = _cache.Get<List<Story>>("storiesKey");

            if (cachedStories == null)
            {
                stories = _service.GetStories().Result;
                AddStoriesToCache(stories);
            }
            else
            {
                stories = cachedStories;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                stories = stories.Where(s => s.Title.Contains(searchString));
            }

            return View(stories);
        }

        private void AddStoriesToCache(IEnumerable<Story> stories)
        {
            MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
            cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            cacheExpirationOptions.Priority = CacheItemPriority.Normal;
            _cache.Set("storiesKey", stories, cacheExpirationOptions);
        }

    }
}