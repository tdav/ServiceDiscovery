using System;
using System.Threading.Tasks;

namespace ServiceDiscovery
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var client = new NSwag();
            client.GetDocument().GetAwaiter().GetResult();
        }
    }
}