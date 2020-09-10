using System;
using System.Threading;
using Books.API.FakeData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Books.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // throttle the thread pool (set available threads to amount of processors)
            ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);

            CreateHostBuilder(args).Build().Run();

            // var authors = Data.GenerateFakeData(2);

            // foreach (var item in authors.AuthorsFake)
            // {
            //     Console.WriteLine(item.Id + " " + item.FirstName + " " + item.LastName);
            // }

            // foreach (var item in authors.BooksFake)
            // {
            //     Console.WriteLine(item.Id + " " + item.Title + " " + item.Description);
            //     Console.WriteLine(item.AuthorId + " " + item.Author.Id);
            // }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
