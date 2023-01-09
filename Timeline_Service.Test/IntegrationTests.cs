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
                        services.AddSingleton<IRedisService, RedisService>();
                    });
                });

            httpClient = application.CreateClient();


            domain = "https://dev-cw2ey26g5kxfxz8s.eu.auth0.com/";
            audience = "https://KwetterNet.com";
        }

        private async Task<string> GetAccessToken()
        {
            AuthenticationApiClient auth0Client =
            new AuthenticationApiClient(new Uri(domain));
            var tokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = "6Zm6rIE8NOF8jAycBPUWaeIjdnczth0E",
                ClientSecret = "0Z8OKpqvk4c6GLNnhcbI6eUzyiAhZCfP4HV5LWsrc84x-IyYW664SqbD_8Lf9pun",
                Audience = audience
            };
            var tokenResponse = await auth0Client.GetTokenAsync(tokenRequest);

            return tokenResponse.AccessToken;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/test/moderator")]
        public async Task Unauthorized(string url)
        {
            // Act
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/timeline" + url);
            var response = await httpClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("/")]
        public async Task getTimeline(string url)
        {
            // Act
            var accessToken = await GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/timeline" + url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await httpClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}

