using Common;
using System;
using MessagePublisherWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace MessagePublisherWebApi.Controllers
{
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessageQueueRepo _queueRepo;

        public MessageController(ILogger<MessageController> logger, IMessageQueueRepo queueRepo)
        {
            _logger = logger;
            _queueRepo = queueRepo;
        }

        //Publish Message
        [HttpPost]
        [Route("[controller]/MessagePublish")]
        public async Task<ActionResult> MessagePublish([FromBody] CalcRequest message)
        {
            try
            {
                _logger.LogInformation($"Publishing message: {message.calcRequestId}");
                await _queueRepo.MessagePublishAsync(message);
                _logger.LogInformation("Successfully published message");
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        //Subscribe Message
        [HttpPost]
        [Route("[controller]/MessageDequeue")]
        public async Task<ActionResult<CalcRequest>> MessageDequeue()
        {
            try
            {
                _logger.LogInformation("Dequeuing message");
                CalcRequest dequeueMessage = await _queueRepo.MessageDequeueAsync();
                if (dequeueMessage != null)
                {
                    _logger.LogInformation($"Dequeued message: {dequeueMessage.calcRequestId}");
                    return Ok(dequeueMessage);
                }
                else
                {
                    _logger.LogInformation("Message queue empty");
                    return NotFound("Message queue empty");
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
