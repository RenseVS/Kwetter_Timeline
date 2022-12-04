using System;
namespace Timeline_Service.MessageContracts
{
	public record TweetDeleted
	{
		public string TweetID { get; init; }
	}
}

