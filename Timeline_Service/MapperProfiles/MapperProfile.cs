using System;
using AutoMapper;
using MessageContracts;
using Timeline_Service.DTOs;
using Timeline_Service.Entities;

namespace Timeline_Service.MapperProfiles
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
            CreateMap<Tweet, TweetDTO>()
               .ReverseMap();
            CreateMap<TweetMadeWithFollower, TweetDTO>();
        }
	}
}

