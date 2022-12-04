using System;
namespace MessageContracts
{
	public record UserCreated
	{
		public string UserID { get; init; }
	}
}

