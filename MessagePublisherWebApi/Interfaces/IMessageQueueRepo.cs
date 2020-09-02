using Common;
using System.Threading.Tasks;

namespace MessagePublisherWebApi.Interfaces
{
    public interface IMessageQueueRepo
    {
        Task MessagePublishAsync(CalcRequest message);
        Task<CalcRequest> MessageDequeueAsync();

        //synchronous -- need to ensure these are complete
        void DeleteQueue();
        void CreateQueue();
        void ClearMessages();
    }
}
