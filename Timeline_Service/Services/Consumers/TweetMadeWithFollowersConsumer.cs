using System;
using AutoMapper;
using MassTransit;
using MessageContracts;
using Timeline_Service.DTOs;

namespace Timeline_Service.Services.Consumers
{
	public class TweetMadeWithFollowerConsumer : IConsumer<TweetMadeWithFollower>
	{
        private readonly TimelineService _timelineService;
        private readonly IMapper _mapper;
        public TweetMadeWithFollowerConsumer(TimelineService timelineService, IMapper mapper)
        {
            _timelineService = timelineService;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<TweetMadeWithFollower> context)
        {
            await _timelineService.UpdateTimeline(context.Message.FollowingUserID, _mapper.Map<TweetMadeWithFollower, TweetDTO>(context.Message));
        }
    }
}

