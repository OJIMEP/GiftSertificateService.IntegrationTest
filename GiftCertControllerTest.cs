using FluentAssertions;
using GiftCertificateService.Contracts.V1.Responses;

namespace GiftSertificateService.IntegrationTest
{
    public class GiftCertControllerTest : IntegrationTest
    {
        [Fact]
        public async Task GetInfoAsync_WithValidBarcode()
        {
            // Arrange 
            await AuthenticateAsync();

            // Act 
            var response = await TestClient.GetAsync("api/GiftCert?barcode=CC13AVC5Yrw");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var result = await response.Content.ReadAsAsync<CertGetResponse>();
            result.Barcode.Should().Be("CC13AVC5Yrw");
        }

        [Theory]
        [InlineData("api/GiftCert?barcode=CC13AVC5YRK", "Certs aren't valid")]
        [InlineData("api/GiftCert?barcode=CC13AVC5YRK1", "Cert's barcode should be 11 symbols length")]
        [InlineData("api/GiftCert?barcode=CC13AVC5Kдг", "Cert's barcode is in wrong format - only latin symbols and digits are allowed")]
        public async Task GetInfoAsync_WithInvalidBarcode(string query, string expected)
        {
            // Arrange 
            await AuthenticateAsync();

            // Act 
            var response = await TestClient.GetAsync(query);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            (await response.Content.ReadAsAsync<ErrorResponse>()).Error.Should().Be(expected);
        }
    }
}
