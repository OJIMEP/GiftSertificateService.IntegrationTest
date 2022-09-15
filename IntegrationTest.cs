using DateTimeService.Areas.Identity.Models;
using GiftCertificateService;
using GiftCertificateService.Contracts.V1.Responses;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;

namespace GiftSertificateService.IntegrationTest
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Program>();
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync("api/Authenticate/login", new LoginModel
            {
                Username = "admin@test.com",
                Password = "qwert1"
            });

            var loginResponse = await response.Content.ReadAsAsync<LoginResponse>();
            return loginResponse.Token;
        }
    }
}
