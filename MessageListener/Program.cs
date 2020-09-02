using System;
using System.IO;
using System.Threading;
using MessageListener.Listener;
using Common;
using System.Text;

namespace MessageListener
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Listener");

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            MQSimulatorListenerHttp listener = new MQSimulatorListenerHttp();
            listener.Subscribe<CalcRequest>(StartWork, token);

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

            source.Cancel();
            listener.Close();
        }

        static void StartWork(CalcRequest calcRequest)
        {
            Console.WriteLine("Processing message {0}", calcRequest.calcRequestId);
            byte[] data = Encoding.ASCII.GetBytes(calcRequest.serializedLargeData);
            Worker worker = new Worker();
            worker.DoWorkAsync(calcRequest.calcRequestId, data).Wait();
        }
    }
}
