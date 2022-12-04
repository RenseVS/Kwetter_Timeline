using System;
namespace MessageContracts
{
	public record UserStoppedFollowing
	{
		public string UserID { get; init; }
		public string StoppedFollowingID { get; init; }
	}
}

