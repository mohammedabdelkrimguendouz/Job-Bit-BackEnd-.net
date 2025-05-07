using JobBit_Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBit.Controllers
{
    [Route("api/Requests")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        [HttpGet("GetRequestByID/{RequestID}", Name = "GetRequestByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Request.AllRequestInfo> GetRequestByID(int RequestID)
        {
            if (RequestID < 1)
                return BadRequest(new { message = "Invalid Request ID", RequestID });

            Request Request = Request.Find(RequestID);

            if (Request == null)
                return NotFound(new { message = "Request not found", RequestID });

            return Ok(Request.allRequestInfo);
        }
    }
}
