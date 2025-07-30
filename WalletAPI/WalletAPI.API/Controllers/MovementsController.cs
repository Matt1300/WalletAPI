using Microsoft.AspNetCore.Mvc;
using WalletAPI.Application.DTOs;
using WalletAPI.Application.Movements.Create;
using WalletAPI.Application.Movements.Get;
using WalletAPI.Application.Shared;

namespace WalletAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovementsController : ControllerBase
    {
        private readonly ICommandHandler<TransferBalanceCommand, Result> _transferBalanceHandler;
        private readonly IQueryHandler<GetAllTransferQuery, List<MovementsDto>> _getAllTransfersHandler;

        public MovementsController(
        ICommandHandler<TransferBalanceCommand, Result> transferBalanceHandler,
        IQueryHandler<GetAllTransferQuery, List<MovementsDto>> getAllTransfersHandler)
        {
            _transferBalanceHandler = transferBalanceHandler;
            _getAllTransfersHandler = getAllTransfersHandler;
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
                return Ok(result.Message);
            }

            return BadRequest(new { result.Message, result.Errors });
        }

        /// <summary>
        /// Obtener todas las transferencias realizadas.
        /// </summary>
        /// <returns>Transferencias realizadas</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllTransfers(CancellationToken cancellationToken)
        {
            var query = new GetAllTransferQuery();
            var transfers = await _getAllTransfersHandler.Handle(query, cancellationToken);

            if (transfers != null)
            {
                return Ok(transfers);
            }

            return NotFound(new { Message = "No se encontraron transferencias." });

        }
    }
}
