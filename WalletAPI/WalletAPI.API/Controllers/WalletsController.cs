using Microsoft.AspNetCore.Mvc;
using WalletAPI.Application.DTOs;
using WalletAPI.Application.Movements.Create;
using WalletAPI.Application.Shared;
using WalletAPI.Application.Wallets.Create;
using WalletAPI.Application.Wallets.GetById;

namespace WalletAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletsController : ControllerBase
{
    private readonly ICommandHandler<CreateWalletCommand, Result<int>> _createWalletHandler;
    private readonly IQueryHandler<GetWalletByIdQuery, Result<WalletDto>> _getWalletByIdHandler;
    private readonly ICommandHandler<TransferBalanceCommand, Result> _transferBalanceHandler;

    public WalletsController(
        ICommandHandler<CreateWalletCommand, Result<int>> createWalletHandler,
        IQueryHandler<GetWalletByIdQuery, Result<WalletDto>> getWalletByIdHandler,
        ICommandHandler<TransferBalanceCommand, Result> transferBalanceHandler)
    {
        _createWalletHandler = createWalletHandler;
        _getWalletByIdHandler = getWalletByIdHandler;
        _transferBalanceHandler = transferBalanceHandler;
    }

    /// <summary>
    /// Crea una nueva billetera.
    /// </summary>
    /// <param name="command">Datos para crear la billetera.</param>
    /// <returns>El ID de la billetera creada.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)] // Para DocumentId duplicado
    public async Task<IActionResult> CreateWallet([FromBody] CreateWalletCommand command, CancellationToken cancellationToken)
    {
        var result = await _createWalletHandler.Handle(command, cancellationToken);

        if (result.Succeeded)
        {
            return StatusCode(StatusCodes.Status201Created, new { Id = result.Value, Message = result.Message });
        }

        if (result.Message.Contains("already exists"))
        {
            return Conflict(new { Message = result.Message, Errors = result.Errors });
        }
        return BadRequest(new { Message = result.Message, Errors = result.Errors });
    }

    /// <summary>
    /// Obtiene una billetera por su ID.
    /// </summary>
    /// <param name="id">ID de la billetera.</param>
    /// <returns>La billetera solicitada.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWalletById(int id, CancellationToken cancellationToken)
    {
        var query = new GetWalletByIdQuery { Id = id };
        var result = await _getWalletByIdHandler.Handle(query, cancellationToken);

        if (result.Succeeded)
        {
            return Ok(result.Value);
        }

        return NotFound(new { Message = result.Message, Errors = result.Errors });
    }

    /// <summary>
    /// Realiza una transferencia de saldo entre billeteras.
    /// </summary>
    /// <param name="command">Datos de la transferencia.</param>
    /// <returns>Confirmación de la transferencia.</returns>
    [HttpPost("transfer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> TransferBalance([FromBody] TransferBalanceCommand command, CancellationToken cancellationToken)
    {
        var result = await _transferBalanceHandler.Handle(command, cancellationToken);

        if (result.Succeeded)
        {
            return Ok(new { Message = result.Message });
        }

        if (result.Message.Contains("not found"))
        {
            return NotFound(new { Message = result.Message, Errors = result.Errors });
        }
        if (result.Message.Contains("Insufficient balance"))
        {
            return Conflict(new { Message = result.Message, Errors = result.Errors });
        }
        return BadRequest(new { Message = result.Message, Errors = result.Errors });
    }
}
