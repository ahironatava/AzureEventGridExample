using EventGridPublisher;
using FacadeApi.Interfaces;
using FacadeApi.Models;

namespace FacadeApi.Services
{
    public class FacadeService : IFacadeService
    {
        private readonly IConfiguration _configuration;
        private readonly EventGridClient _eventGridClient;

        public FacadeService(IConfiguration configuration)
        {
            _configuration = configuration;

            string? topicEndpoint = _configuration["EventGridTopicEndpoint"];
            string? topicKey = _configuration["EventGridTopicKey"];

            if(string.IsNullOrWhiteSpace(topicEndpoint) || string.IsNullOrWhiteSpace(topicKey))
            {
                throw new ArgumentException("Event Grid Topic Endpoint and Key must be provided.");
            }

            _eventGridClient = new EventGridClient(
                               topicEndpoint,
                               topicKey);
        }

        public (string errMsg, int recordId) ProcessRequest(string value)
        {
            // Store the content in the database and capture the record ID created (or resulting error message).
            // Do this synchronously as a failure to write should be reported to the user immediately.
            // (Faking, as unnecessary for this demo)
            string _errorMessage = string.Empty;
            int _recordId = 123456789;

            // Create a payload to send to the Event Grid Topic: decorate the input with the record ID and current time.
            var currentDateTime = DateTime.UtcNow;
            var dummyPayload = new StoredRecord { 
                UserString = value, 
                RecordId = _recordId,
                Hour = currentDateTime.Hour, 
                Minute = currentDateTime.Minute, 
                Second = currentDateTime.Second
            };

            // Raise an Event for publishing, but don't block the caller waiting for it to be sent.
            var _ =  _eventGridClient.PublishEventGridEvent("recordId", "NewRequest", dummyPayload);

            // Notify the user of the record ID or error message when creating the record.
            return (_errorMessage, _recordId);
        }

        public (int statusCode, string statusMessage, StoredRecord record) GetRecord(int id)
        {
            // Check the database for the record ID.
            // If the record ID is not found, return 404 with message "File not Found".
            // If the record ID is found but not processed, return 202 with message "Result not Ready".
            // If the record ID is processed, return the result with status code of 200 and an empty message.
            // (Faking, as unnecessary for this demo)
            int _statusCode = 200;
            string _statusMessage = string.Empty;

            var currentDateTime = DateTime.UtcNow;
            var dummyRecord = new StoredRecord
            {
                RecordId = id,
                UserString = "user string",
                Hour = currentDateTime.Hour,
                Minute = currentDateTime.Minute,
                Second = currentDateTime.Second
            };

            return (_statusCode, _statusMessage, dummyRecord);
        }
    }
}
