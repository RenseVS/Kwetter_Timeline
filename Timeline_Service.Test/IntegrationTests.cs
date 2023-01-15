using System;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Timeline_Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using System.Net;
using System.Net.Http.Headers;
using Timeline_Service.Entities;
using Timeline_Service.DTOs;
using System.Text.Json;

namespace Timeline_Service.Test
{
	public class IntegrationTests
	{
        private readonly HttpClient httpClient;
        private readonly string clientId;
        private readonly string domain;
        private readonly string audience;

        public IntegrationTests()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IRedisService, RedisServiceMock>();
                    });
                });

            httpClient = application.CreateClient();


            domain = "https://dev-cw2ey26g5kxfxz8s.eu.auth0.com/";
            audience = "https://KwetterNet.com";
        }

        /// <summary>
        /// User 1 has been setup to retrieve his own timline from the cache and merge that with new celeberty tweets since it has been over 5 minutes since that happpened
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TimelineWithCelebUpdate()
        {
            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/timeline/test/moderator/1");
            var response = await httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            List<TweetDTO> tweets = JsonSerializer.Deserialize<List<TweetDTO>>(body);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //The assert means that it is succesfull in merging the timeline with the celeberty tweets since it is set up with 1 item in the cache and 1 new celeberty tweet
            Assert.Equal(2, tweets.Count());
        }

        /// <summary>
        /// User 2 has been setup to retrieve his own timline from the cache and not merge that with new celeberty tweets since it hasn't been over 5 minutes since that happpened
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TimelineWithoutCelebUpdate()
        {
            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/timeline/test/moderator/2");
            var response = await httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            List<TweetDTO> tweets = JsonSerializer.Deserialize<List<TweetDTO>>(body);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //The assert means that it is succesfull and that it hasnt merged the timeline with the celeberty tweets since it is set up with 1 item in the cache and 1 new celeberty tweet
            Assert.Single(tweets);
        }

        /// <summary>
        /// User 3 has been set up to retrieve his timeline from the database wich has 1 item with id 0
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TimelineFromRegularDB()
        {
            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/timeline/test/moderator/3");
            var response = await httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            List<TweetDTO> tweets = JsonSerializer.Deserialize<List<TweetDTO>>(body);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //The assert means that it is succesfull and that it hasnt merged the timeline with the celeberty tweets since it is set up with 1 item in the cache and 1 new celeberty tweet
            Assert.Single(tweets);
            //The assert means that it is succesfull since the db tweet that is setup has an id of 0;
            Assert.Equal("0", tweets[0].tweetID);

        }
    }
}

