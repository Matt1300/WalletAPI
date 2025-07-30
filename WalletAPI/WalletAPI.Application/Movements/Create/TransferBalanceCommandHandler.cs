using WalletAPI.Application.Interfaces;
using WalletAPI.Application.Shared;
using WalletAPI.Domain.Entities;

namespace WalletAPI.Application.Movements.Create;

public class TransferBalanceCommandHandler(
    IWalletRepository _walletRepository,
    IMovementRepository _movementRepository)
    : ICommandHandler<TransferBalanceCommand, Result>
{
    public async Task<Result> Handle(TransferBalanceCommand command, CancellationToken cancellationToken)
    {
        var sourceWallet = await _walletRepository.GetByIdAsync(command.SourceWalletId, cancellationToken);
        var destinationWallet = await _walletRepository.GetByIdAsync(command.DestinationWalletId, cancellationToken);

        var validationResult = Validate(command, sourceWallet, destinationWallet);
        if (!validationResult.Succeeded)
            return validationResult;

        sourceWallet.Debit(command.Amount);
        destinationWallet.Credit(command.Amount);

        var debitMovement = new Movement(sourceWallet.Id, command.Amount, MovementType.Debit);
        var creditMovement = new Movement(destinationWallet.Id, command.Amount, MovementType.Credit);

        try
        {
            await _walletRepository.UpdateAsync(sourceWallet, cancellationToken);
            await _walletRepository.UpdateAsync(destinationWallet, cancellationToken);
            await _movementRepository.AddAsync(debitMovement);
            await _movementRepository.AddAsync(creditMovement);
        }
        catch (Exception ex)
        {
            return Result.Failure("Un error ocurrió durante la transferencia.", [ex.Message]);
        }

        return Result.Success("Transferencia completada con éxito.");
    }

    private Result Validate(TransferBalanceCommand command, Wallet sourceWallet, Wallet destinationWallet)
    {
        if (command.SourceWalletId == command.DestinationWalletId)
        {
            return Result.Failure("No se puede transferir saldo a la misma billetera.", ["El destino de la transferencia no puede ser la misma billetera"]);
        }

        if (command.Amount <= 0)
        {
            return Result.Failure("El monto a transferir debe ser mayor que cero.", ["El monto debe ser positivo"]);
        }

        if (sourceWallet == null)
        {
            return Result.Failure("No se encontró la billetera de origen.", ["La billetera de origen no existe."]);
        }

        if (sourceWallet.Balance < command.Amount)
        {
            return Result.Failure("Saldo insuficiente en la billetera de origen.", ["El saldo de la billetera de origen es insuficiente para realizar la transferencia."]);
        }

        if (destinationWallet == null)
        {
            return Result.Failure("No se encontró la billetera de destino.", ["La billetera de destino no existe."]);
        }

        return Result.Success();
    }
}
