using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common;
using MessagePublisher.Publisher;

namespace MessagePublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            string calcRequestSourcePath = ".\\IncomingRequests";
            string calcRequestDestinationPath = ".\\Storage";

            if (!Directory.Exists(calcRequestSourcePath))
            {
                Directory.CreateDirectory(calcRequestSourcePath);
            }
            if (!Directory.Exists(calcRequestDestinationPath))
            {
                Directory.CreateDirectory(calcRequestDestinationPath);
            }

            MQSimulatorClientHttp messageClient = new MQSimulatorClientHttp();

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Task task = Task.Run(() =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        var files = Directory.GetFiles(calcRequestSourcePath);
                        if (files.Length == 0)
                        {
                            Console.WriteLine("Waiting for new calc requests");
                            Thread.Sleep(1000);
                        }

                        foreach (var file in files)
                        {
                            Console.WriteLine("Processing '{0}'", Path.GetFileName(file));

                            string[] inputData = File.ReadAllLines(file);
                            {
                                //Run some data validation
                            }

                            for (int i = 1; i < inputData.Length; i++)
                            {
                                var calcRequest = CalcRequestExtensions.FromSourceCsvFile(inputData[i]);
                                File.WriteAllText(Path.Combine(calcRequestDestinationPath, calcRequest.calcRequestId.ToString()), calcRequest.serializedLargeData);
                                messageClient.PublishAsync(calcRequest).Wait();
                            }

                            File.Delete(file);
                        }
                    }
                });

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();

            source.Cancel();
            task.Wait();
        }
    }
}
