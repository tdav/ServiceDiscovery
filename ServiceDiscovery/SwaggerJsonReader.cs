using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ServiceDiscovery
{
    public class SwaggerJsonReader
    {
        private string ApiServerUrl = "https://gubdd.uz/mab/swagger/v1/swagger.json";
        private HttpClient _httpClient;

        public SwaggerJsonReader()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _httpClient = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });

            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("OpenApi.Net.Tests", "1.0"));
        }

        public async Task<OpenApiDocument> GetDocument()
        {
            var response = await _httpClient.GetAsync(ApiServerUrl);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Couldn't load {ApiServerUrl}");
                return null;
            }

            await response.Content.LoadIntoBufferAsync();
            var stream = await response.Content.ReadAsStreamAsync();

            var reader = new OpenApiStreamReader();
            var openApiDocument = reader.Read(stream, out var diagnostic);

            if (diagnostic.Errors.Count > 0)
            {
                Console.WriteLine($"Errors parsing {ApiServerUrl}");
                Console.WriteLine(String.Join("\n", diagnostic.Errors));
            }

            return openApiDocument;
        }
    }
}
