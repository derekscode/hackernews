using System.Runtime.Serialization;

namespace HackerNews.Models
{
    public class Story
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "by")]
        public string By { get; set; }
    }
}