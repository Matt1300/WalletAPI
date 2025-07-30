using Microsoft.AspNetCore.Mvc;
using WalletAPI.Application.DTOs;
using WalletAPI.Application.Shared;
using WalletAPI.Application.Wallets.Create;
using WalletAPI.Application.Wallets.Delete;
using WalletAPI.Application.Wallets.GetById;
using WalletAPI.Application.Wallets.Update;

namespace WalletAPI.API.Controllers;

[Route("api/wallets")]
[ApiController]
public class WalletsController(
    ICommandHandler<CreateWalletCommand, Result<int>> _createWalletHandler,
    ICommandHandler<DeleteWalletCommand, Result> _deleteWalletHandler,
    IQueryHandler<GetWalletByIdQuery, Result<WalletDto>> _getWalletByIdHandler,
    ICommandHandler<UpdateWalletCommand, Result> _updateWalletHandler)
    : ControllerBase
{
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
            return Ok(new { result.Message, result.Value });
        }

        return NotFound(new { result.Message, result.Errors });
    }

    /// <summary>
    /// Elimina una billetera por su ID.
    /// </summary>
    /// <param name="id">ID de la billetera.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWallet(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteWalletCommand(id);
        var result = await _deleteWalletHandler.Handle(command, cancellationToken);
        if (result.Succeeded)
        {
            return Ok(new { result.Message });
        }
        return NotFound(new { result.Message, result.Errors });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateWallet(int id, [FromBody] UpdateWalletCommand command, CancellationToken cancellationToken)
    {
        var updateCommand = command with { Id = id };

        var result = await _updateWalletHandler.Handle(updateCommand, cancellationToken);
        if (result.Succeeded)
        {
            return Ok(new { result.Message });
        }
        return NotFound(new { result.Message, result.Errors });

    }
}
