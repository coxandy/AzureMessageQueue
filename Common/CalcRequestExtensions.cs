using System;

namespace Common
{
    public static class CalcRequestExtensions
    {

        public static CalcRequest FromString(this String stringRequest)
        {
            string[] parts = stringRequest.Split(',');

            Guid requestId = Guid.Parse(parts[0]);
            string sourceId = parts[1];
            string userId = parts[2];
            string timeStamp = parts[3];
            string serializedLargeData = parts[4];

            var request = new CalcRequest()
            {
                calcRequestId = requestId,
                sourceSystemId = sourceId,
                timeStamp = DateTime.Parse(timeStamp),
                userId = userId,
                serializedLargeData = serializedLargeData
            };
            return request;
        }

        public static CalcRequest FromSourceCsvFile(this String stringRequest)
        {
            string[] parts = stringRequest.Split(',');

            string sourceId = parts[0];
            string userId = parts[1];
            string timeStamp = parts[2];
            string serializedLargeData = parts[3];
            Guid requestId = Guid.NewGuid();

            var request = new CalcRequest()
            {
                calcRequestId = requestId,
                sourceSystemId = sourceId,
                timeStamp = DateTime.Parse(timeStamp),
                userId = userId,
                serializedLargeData = serializedLargeData
            };

            return request;
        }
    }
}
