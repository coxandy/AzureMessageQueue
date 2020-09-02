using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MessageListener.Listener
{
    public class MQSimulatorListenerHttp
    {
        private List<Task> _runningTasks;        
        private readonly Uri _url;

        public MQSimulatorListenerHttp()
        {
            _runningTasks = new List<Task>();
            _url = new Uri("https://localhost:5001/Message/MessageDequeue");
        }

        public void Close()
        {
            Task.WaitAll(_runningTasks.ToArray());
        }
       
        public void Subscribe<T>(Action<T> callBack, CancellationToken cancellationToken)
        {
            _runningTasks.Add(Task.Factory.StartNew(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var response = HttpSubscribeMessage();
                        var data = response.Result;
                        if (!String.IsNullOrEmpty(data))
                        {
                            T msg = JsonSerializer.Deserialize<T>(data);
                            Console.WriteLine("Received message {0}", data);
                            callBack(msg);
                        }
                        else
                        {
                            Console.WriteLine("Waiting for messages...");
                            Thread.Sleep(1000);
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error subscribing: {ex.Message}");
                    }
                }
                Console.WriteLine("Stopping listener...");
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current));
        }
        
        private async Task<string> HttpSubscribeMessage()
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.PostAsync(_url, null);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;            
        }
    }
}
