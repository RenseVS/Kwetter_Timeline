using System;
using Timeline_Service.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace Timeline_Service.Services
{
	public class CelebertyService
	{
		public CelebertyService()
		{
		}

		public async Task<IEnumerable<Tweet>> GetCelebTweets(string userid)
		{
			var followingCelebs = await GetFollowingCelebs(userid);
            //make api call to get celeberty's usertimeline since this contains their most recent tweets
			Tweet tweet = new Tweet()
            {
                tweetID = "0",
                userName = "Kwetter inc.",
                message = "This is a celeberty tweet made on" + DateTime.Now.ToString(),
                tweetDate = DateTime.Now

            };
            return new List<Tweet>() { tweet };
        }

        private async Task<IEnumerable<string>> GetFollowingCelebs(string userid)
        {
			return new List<string> { "42", "1"};
        }
    }
}

