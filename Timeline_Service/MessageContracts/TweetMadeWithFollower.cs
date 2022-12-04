using System;
namespace MessageContracts
{
	public record TweetMadeWithFollower
	{
		public string TweetID { get; init; }
		public string UserName { get; init; }
		public string Message { get; init; }
        public DateTime TweetDate { get; set; }
        public string FollowingUserID { get; init; }
	}
}

