using System;
using System.Net.Http;
using Books.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Books.Tests.BooksAPITests.WebAppFactory
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public IntegrationTests()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                    BaseAddress = new Uri("https://localhost:5001/api/")
                });
        }
    }
}