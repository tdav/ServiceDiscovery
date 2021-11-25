
using ServiceDiscovery;

Console.WriteLine("Hello, World!");

var client = new SwaggerJsonReader();
await client.GetDocument(); 