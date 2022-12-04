using System;
using MassTransit;
using MessageContracts;

namespace Timeline_Service.Services.Consumers
{
	public class UserCreatedConsumer : IConsumer<UserCreated>
	{
		public UserCreatedConsumer()
		{
		}

        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            //do shit
        }
    }
}

