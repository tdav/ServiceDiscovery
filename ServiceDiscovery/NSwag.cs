using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceDiscovery
{
    public class NSwag
    {
        private string ApiServerUrl = "https://gubdd.uz/mab/swagger/v1/swagger.json";
        private HttpClient _httpClient;

        public NSwag()
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

        public async Task GetDocument()
        {
            var response = await _httpClient.GetAsync(ApiServerUrl);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Couldn't load {ApiServerUrl}");
                return;
            }


            await response.Content.LoadIntoBufferAsync();
            var str = await response.Content.ReadAsStringAsync();

            var document = await OpenApiDocument.FromJsonAsync(str);

            var settings = new CSharpClientGeneratorSettings
            {
                ClassName = "MyClass",
                CSharpGeneratorSettings = { Namespace = "MyNamespace" },
                GenerateDtoTypes = false,


                ExposeJsonSerializerSettings = false,
                GenerateClientClasses = true,
                GenerateClientInterfaces = true,
                GeneratePrepareRequestAndProcessResponseAsAsyncMethods = false,
                InjectHttpClient = true,
                GenerateBaseUrlProperty = false,
                UseBaseUrl = false,
                UseRequestAndResponseSerializationSettings = false,
                DisposeHttpClient = false,
                UseHttpClientCreationMethod = true,
                GenerateOptionalParameters = false,
                
               // OperationNameGenerator = new OperationNameGenerator(),
            };



            var generator = new CSharpClientGenerator(document, settings);


            var code = generator.GenerateFile();
        }
    }


    //public class OperationNameGenerator : IOperationNameGenerator
    //{
    //    public bool SupportsMultipleClients => true;

    //    public string GetClientName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation)
    //    {
    //        var cn = path.Split('/');
    //        return cn[0];
    //    }

    //    public string GetOperationName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation)
    //    {
    //        return path.Replace('/', '_');
    //    }
    //}
}
