using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace Lesson_1
{
    class Program
    {
        static async Task<HttpResponseMessage> GetHttpResponseAsync(string uriString)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                return await httpClient.GetAsync(uriString);
            }
            catch (Exception)
            {
                Console.WriteLine("Http client error!");
                return null;
            }
            
        }
        static void WritePostToFile(Post post)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "result.txt");
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(post.userId);
                streamWriter.WriteLine(post.id);
                streamWriter.WriteLine(post.title);
                streamWriter.WriteLine(post.body);
                streamWriter.WriteLine("");
            }
        }
        static void Main(string[] args)
        {
            var tasks = new Task<HttpResponseMessage>[10];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = GetHttpResponseAsync("https://jsonplaceholder.typicode.com/posts/" + (i+4));
            }
            Task.WaitAll(tasks);
            foreach (var task in tasks)
            {
                if (task.Result != null)
                {
                    WritePostToFile(JsonSerializer.Deserialize<Post>(task.Result.Content.ReadAsStringAsync().Result));
                }
            }
            Console.WriteLine("End of main");
        }
    }
}
