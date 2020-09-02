using System;

namespace Common
{
    public class CalcRequest
    {
        public Guid calcRequestId { get; set; }
        public string sourceSystemId { get; set; }
        public string userId { get; set; }
        public DateTime timeStamp { get; set; }
        public string serializedLargeData { get; set; }
    
        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3},{4}", calcRequestId.ToString(), 
                                                        sourceSystemId, 
                                                        userId, 
                                                        timeStamp.ToString("dd MMM yyyy hh:mm:ss"),
                                                        serializedLargeData);
        }
    }
}
