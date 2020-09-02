using System;
using System.Threading.Tasks;

namespace MessageListener
{
    public class Worker
    {
        public async Task<bool> DoWorkAsync(Guid requestId, byte[] requestData)
        {
            await Task.Run(() =>
            {
                // Do complex work
            });

            return true;
        }
    }
}
