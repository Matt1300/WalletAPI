using Microsoft.AspNetCore.Mvc;
using WalletAPI.Application.Movements.Create;
using WalletAPI.Application.Shared;

namespace WalletAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovementsController : ControllerBase
    {
        private readonly ICommandHandler<TransferBalanceCommand, Result> _transferBalanceHandler;

        public MovementsController(
        ICommandHandler<TransferBalanceCommand, Result> transferBalanceHandler)
        {
            _transferBalanceHandler = transferBalanceHandler;
        }

        /// <summary>
        /// Realiza una transferencia de saldo entre billeteras.
        /// </summary>
        /// <param name="command">Datos de la transferencia.</param>
        /// <returns>Confirmación de la transferencia.</returns>
        [HttpPost("transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TransferBalance([FromBody] TransferBalanceCommand command, CancellationToken cancellationToken)
        {
            var result = await _transferBalanceHandler.Handle(command, cancellationToken);

            if (result.Succeeded)
            {
                return Ok(new { result.Message });
            }

            return BadRequest(new { result.Message, result.Errors });
        }
    }
}
