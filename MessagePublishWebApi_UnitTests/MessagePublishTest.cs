using System;
using Xunit;
using Moq;
using MessagePublisherWebApi.Model;
using Microsoft.Extensions.Options;
using MessagePublisherWebApi.Controllers;
using Microsoft.Extensions.Logging;
using MessagePublisherWebApi.MessageQueueRepo;
using Microsoft.AspNetCore.Mvc;
using Common;
using System.Text;

namespace MessageControllerWebApi_UnitTests
{
    public class MessagePublishTest
    {
        private readonly MessageController _controller;
        private readonly AzureStorageQueueRepo _mockAzureQueueRepo;
        private readonly Random _random;

        public MessagePublishTest()
        {
            //mock app settings
            PublisherSettings mockAppSettings = new PublisherSettings() 
            {  
                AzureConnection = "DefaultEndpointsProtocol=https;AccountName=lbgazuretest;AccountKey=ZV1uWrheTKv9xPh68TTzxNYypdHfqNQEmhVSWdrMVZnU5Iml7F218g62GPkw2Sg9cXtZ4zxLBid8naBVbUYjFg==;EndpointSuffix=core.windows.net", 
                QueueName = "testqueuecox1"
            };

            _random = new Random();
            var mockLogger = new Mock<ILogger<MessageController>>();
            var mockOptions = new Mock<IOptions<PublisherSettings>>();
            mockOptions.Setup(ap => ap.Value).Returns(mockAppSettings);

            _mockAzureQueueRepo = new AzureStorageQueueRepo(mockOptions.Object);
            _controller = new MessageController(mockLogger.Object, _mockAzureQueueRepo);
        }


        [Fact]
        public async void TestPublishAndDequeue()
        {
            //Clear massages
            _mockAzureQueueRepo.ClearMessages();

            //create random message
            CalcRequest randomMessage = MockRandomCalcRequest();

            //publish msg
            await _mockAzureQueueRepo.MessagePublishAsync(randomMessage);

            //dequeue msg
            CalcRequest testDequeueMmessage = await _mockAzureQueueRepo.MessageDequeueAsync();

            //test equality
            Assert.Equal(randomMessage.calcRequestId, testDequeueMmessage.calcRequestId);
            Assert.Equal(randomMessage.sourceSystemId, testDequeueMmessage.sourceSystemId);
        }


        [Fact]
        public async void TestPublishRandomMessages()
        {
            //Clear massages
            _mockAzureQueueRepo.ClearMessages();

            //Generate and publish lots of random messages
            for (int i=0; i<100; i++)
            {
                CalcRequest randomMessage = MockRandomCalcRequest();
                await _mockAzureQueueRepo.MessagePublishAsync(randomMessage);
            }
            Assert.True(true);
        }


        [Fact]
        public async void TestControllerPublishAndDequeue()
        {
            CalcRequest randomMessage = MockRandomCalcRequest();
            IActionResult response = await _controller.MessagePublish(randomMessage);
            OkResult okResult = response as OkResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
       
        private CalcRequest MockRandomCalcRequest()
        {
            //generate random requests
            int number = _random.Next(1, 100000);
            var testCalcrequest = new CalcRequest();
            testCalcrequest.calcRequestId = Guid.Empty;
            testCalcrequest.sourceSystemId = number.ToString();
            testCalcrequest.userId = RandomString(10);
            testCalcrequest.timeStamp = DateTime.Now;
            testCalcrequest.serializedLargeData = "[ '31 Jan 2020', 'deltasensitivity', false, '1M', 285442990,  11311232.8857107 ]";
            return testCalcrequest;
        }

        private string RandomString(int length)
        {
            var randomstring = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var @char = (char)_random.Next('A', 'A' + 26);
                randomstring.Append(@char);
            }
            return randomstring.ToString();
        }
    }
}
