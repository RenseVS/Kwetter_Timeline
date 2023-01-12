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
            Assert.Equal(2, tweets.Count());
        }

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
            Assert.Single(tweets);
        }

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
            Assert.Single(tweets);
            Assert.Equal("0", tweets[0].tweetID);

        }
    }
}

