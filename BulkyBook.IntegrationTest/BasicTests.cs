
using Xunit;


using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BulkyBook.IntegrationTest
{
    public class BasicTests:IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public BasicTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Admin/Order")]
        public async Task GetHttpRequest(string url)
        {
            //Arrange
            var client = _factory.CreateClient();   

            //Act
            var response = await client.GetAsync(url);  


            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
