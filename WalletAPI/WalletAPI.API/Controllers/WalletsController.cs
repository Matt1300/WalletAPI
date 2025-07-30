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

    public WalletsController(
        ICommandHandler<CreateWalletCommand, Result<int>> createWalletHandler,
        IQueryHandler<GetWalletByIdQuery, Result<WalletDto>> getWalletByIdHandler,
        ICommandHandler<TransferBalanceCommand, Result> transferBalanceHandler)
    {
        _createWalletHandler = createWalletHandler;
        _getWalletByIdHandler = getWalletByIdHandler;
    }

    /// <summary>
    /// Crea una nueva billetera.
    /// </summary>
    /// <param name="command">Datos para crear la billetera.</param>
    /// <returns>El ID de la billetera creada.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWallet([FromBody] CreateWalletCommand command, CancellationToken cancellationToken)
    {
        var result = await _createWalletHandler.Handle(command, cancellationToken);

        if (result.Succeeded)
        {
            return CreatedAtAction(
                nameof(GetWalletById),
                new { id = result.Value },
                new { id = result.Value, result.Message });
        }
        return BadRequest(new { result.Message, result.Errors });
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
            return Ok(new {result.Message , result.Value});
        }

        return NotFound(new { result.Message, result.Errors });
    }

    
}
