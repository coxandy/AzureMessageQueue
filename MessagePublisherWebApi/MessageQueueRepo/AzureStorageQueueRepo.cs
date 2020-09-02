using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Common;
using MessagePublisherWebApi.Interfaces;
using MessagePublisherWebApi.Model;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;


namespace MessagePublisherWebApi.MessageQueueRepo
{
    public class AzureStorageQueueRepo: IMessageQueueRepo
    {
        private PublisherSettings _settings;
        private string _connectionString;
        private QueueClient _queueClient;

        public AzureStorageQueueRepo(IOptions<PublisherSettings> settings)
        {
            _settings = settings.Value;
            
            //read connection stsring and conect to azure
            _connectionString = _settings.AzureConnection;            
            _queueClient = new QueueClient(_connectionString, _settings.QueueName);

            // Create the queue if it doesn't exist
            _queueClient.CreateIfNotExists();
        }

        public async Task MessagePublishAsync(CalcRequest message)
        {
            try
            {
                if (_queueClient.Exists())
                {
                    //Assign guid & send the full request message as csv string to the queue
                    message.calcRequestId = Guid.NewGuid();
                    var response = await _queueClient.SendMessageAsync(message.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CalcRequest> MessageDequeueAsync()
        {
            try
            {
                if (_queueClient.Exists())
                {
                    //If exists dequeue message and convert to CalcRequest
                    QueueMessage[] retrievedMessage = await _queueClient.ReceiveMessagesAsync();
                    if (retrievedMessage.Length > 0)
                    {
                        string dequeueCalcRequest = retrievedMessage[0].MessageText;
                        CalcRequest response = CalcRequestExtensions.FromString(dequeueCalcRequest);

                        //make sure to remove from Azure queue
                        await _queueClient.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public void DeleteQueue()
        {
            try
            {
                if (_queueClient.Exists())
                {
                    // Delete the queue
                    _queueClient.Delete();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void CreateQueue()
        {
            try
            {              
                _queueClient.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ClearMessages()
        {
            try
            {
                _queueClient.ClearMessages();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
