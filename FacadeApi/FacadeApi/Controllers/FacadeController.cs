using Microsoft.AspNetCore.Mvc;
using FacadeApi.Interfaces;
using FacadeApi.Models;

namespace FacadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacadeController : ControllerBase
    {
        private readonly IFacadeService _facadeService;

        public FacadeController(IFacadeService facadeService)
        {
            _facadeService = facadeService;
        }

        // POST a user request
        [HttpPost]
        public void Post([FromBody] string value)
        {
            // If request is invalid return a 400.
            if (string.IsNullOrWhiteSpace(value))
            {
                Response.StatusCode = 400;
                return;
            }

            // Call the service to process the request
            (string errMsg, int recordId) serviceResult = _facadeService.ProcessRequest(value);

            // If successful, return the ID to use in a GET call.
            if(string.IsNullOrEmpty(serviceResult.errMsg))
            {
                Response.StatusCode = 202;
                Response.Headers.Append("Location", $"api/Facade/{serviceResult.recordId}");
            }
            else
            {
                Response.StatusCode = 500;
            }
        }

        // GET the results for the specified request identfier.
        [HttpGet("{id}")]
        public StoredRecord Get(int id)
        {
            // Call the service to fetch the request.
            (int statusCode, string statusMessage, StoredRecord record) serviceResult = _facadeService.GetRecord(id);
            if (serviceResult.statusCode != 200)
            {
                // If there is an error, return the error code and place the error message in the header.
                Response.StatusCode = serviceResult.statusCode;
                Response.Headers.Append("StatusMessage", serviceResult.statusMessage);
                return new StoredRecord();
            }
            else 
            {
                Response.StatusCode = 200;
                return serviceResult.record;
            }
        }
    }
}
