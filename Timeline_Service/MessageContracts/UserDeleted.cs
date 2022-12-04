using System;
namespace MessageContracts
{
	public record UserDeleted
	{
		public string UserID { get; init; }
	}
}

