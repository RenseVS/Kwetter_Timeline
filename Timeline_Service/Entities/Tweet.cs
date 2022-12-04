using System;
namespace Timeline_Service.Entities
{
	public class Tweet
	{
        public string? TweetID { get; set; }
        public string? UserName { get; set; }
        public string? Message { get; set; }
        public DateTime TweetDate { get; set; }
    }
}

