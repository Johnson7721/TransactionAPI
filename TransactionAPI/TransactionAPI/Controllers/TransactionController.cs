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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionResponse>> SubmitTransaction([FromBody] TransactionRequest request, CancellationToken cancellationToken)
        {
            var url = HttpContext.Request.Path.ToString();

            Logger.LogRequest(url, request!);

            var response = await _transactionService.ProcessTransactionAsync(request!);

            Logger.LogResponse(url, response);

            return Ok(response);
        }
    }
}
