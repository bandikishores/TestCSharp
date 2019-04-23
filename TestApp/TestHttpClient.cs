using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestApp
{
    class TestHttpClient
    {
        public static void Main1()
        {
            makeAsyncCall();
            Console.ReadKey();
        }

        private async static void makeAsyncCall()
        {
            var random = new Random();
            var parallelLoopResult = Parallel.For(0, 100, new ParallelOptions { MaxDegreeOfParallelism = 1 },
                async i =>
                {
                    var delay = random.Next(500, 5000);
                    var wait = Task.Delay(delay);
                    await wait;
                    Console.WriteLine(i + " with Delay " + delay);
                });

            Console.ReadKey();
            using (HttpClient httpClient = new HttpClient())
            {
                List<Task> taskList = new List<Task>();
                var startTime = DateTime.UtcNow;
                for (int i = 0; i < 1000; i++)
                {
                    taskList.Add(Task.Run(async () =>
                        {
                            HttpRequestMessage msg = new HttpRequestMessage();
                            var httpLocalhostRestSleepSleep = "http://localhost:8283/rest/jersey/sleep?sleep=5000";
                            Uri resultUri = new Uri(httpLocalhostRestSleepSleep);
                            msg.Method = HttpMethod.Get;
                            msg.RequestUri = new Uri(httpLocalhostRestSleepSleep);

                            var result = await httpClient.SendAsync(msg);
                            var ResultCode = result.StatusCode.ToString();
                            Console.WriteLine(ResultCode);
                            if (result.IsSuccessStatusCode)
                            {
                                using (var stream = await result.Content.ReadAsStreamAsync())
                                {
                                    using (var sr = new StreamReader(stream))
                                    {
                                        using (var jr = new JsonTextReader(sr))
                                        {
                                            var token = JObject.ReadFrom(jr);
                                            Console.WriteLine(token);
                                        }
                                    }
                                }
                            }
                        }
                    ));
                }

                await Task.WhenAll(taskList);
                Console.WriteLine("Total Time Taken " + DateTime.UtcNow.Subtract(startTime));
            }

        }
    }
}
