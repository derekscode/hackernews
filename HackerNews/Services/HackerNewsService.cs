using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HackerNews.Models;

namespace HackerNews.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<string>> GetStoryIds();
        Task<IEnumerable<Story>> GetStories();
    }

    public class HackerNewsService : IHackerNewsService
    {
        private HttpClient _client;
        const string baseAddress = "https://hacker-news.firebaseio.com/v0/";

        public HackerNewsService()
        {
            _client = CreateHttpClient();
        }

        public async Task<IEnumerable<string>> GetStoryIds()
        {
            List<string> ids = new List<string>();

            HttpResponseMessage response = await _client.GetAsync("beststories.json");
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                ids = JsonConvert.DeserializeObject<List<string>>(json);
            }
            return ids;
        }

        public async Task<IEnumerable<Story>> GetStories()
        {
            List<string> storyIds = (List<string>)await GetStoryIds();
            List<Story> stories = new List<Story>();

            foreach (var id in storyIds)
            {
                HttpResponseMessage response = await _client.GetAsync("item/" + id + ".json");
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var story = JsonConvert.DeserializeObject<Story>(json);
                    stories.Add(story);
                }
            }

            return stories;
        }

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }


}
