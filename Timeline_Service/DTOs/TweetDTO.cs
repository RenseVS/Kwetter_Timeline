using System;
namespace Timeline_Service.DTOs
{
	public class TweetDTO
	{
        public string tweetID { get; set; }
        public string userName { get; set; }
        public string message { get; set; }
        public DateTime tweetDate { get; set; }
    }
}

