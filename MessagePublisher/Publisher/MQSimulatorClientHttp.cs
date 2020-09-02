using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common;

namespace MessagePublisher.Publisher
{
    public class MQSimulatorClientHttp
    {
        private readonly Uri _url;

        public MQSimulatorClientHttp()
        {
            _url = new Uri("https://localhost:5001/Message/MessagePublish");
        }

        public async Task PublishAsync(CalcRequest keyData)
        {
            await Task.Run(() =>
            {
                HttpContent jsonContent = new StringContent(JsonSerializer.Serialize(keyData), Encoding.UTF8, "application/json");
                var response = HttpPublishMessage(_url, jsonContent);
                if (response.Result.Equals("Ok", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Published message {keyData.calcRequestId}");
                }
            });
        }

        private async Task<string> HttpPublishMessage(Uri u, HttpContent content)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.PostAsync(u, content);
                if (result.IsSuccessStatusCode)
                {
                    response = result.StatusCode.ToString();
                }
            }
            return response;
        }
    }
}
