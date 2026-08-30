using Microsoft.AspNetCore.Mvc;
using TransactionAPI.Helpers.Logger;
using TransactionAPI.Models;
using TransactionAPI.Services;

namespace TransactionAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("submittrxmessage")]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<TransactionResponse> SubmitTransaction([FromBody] TransactionRequest request)
        {
            var url = HttpContext.Request.Path.ToString();

            Logger.LogRequest(url, request);

            var response =  _transactionService.ProcessTransaction(request);

            Logger.LogResponse(url, response);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
