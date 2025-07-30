using WalletAPI.Application.Shared;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Repositories;

namespace WalletAPI.Application.Movements.Create;

public class TransferBalanceCommandHandler(IUnitOfWork _unitOfWork)
    : ICommandHandler<TransferBalanceCommand, Result>
{
    public async Task<Result> Handle(TransferBalanceCommand command, CancellationToken cancellationToken)
    {
        var sourceWallet = await _unitOfWork.Wallets.GetByIdAsync(command.SourceWalletId, cancellationToken);
        var destinationWallet = await _unitOfWork.Wallets.GetByIdAsync(command.DestinationWalletId, cancellationToken);

        var validationResult = Validate(command, sourceWallet, destinationWallet);
        if (!validationResult.Succeeded)
            return validationResult;

        sourceWallet.Debit(command.Amount);
        destinationWallet.Credit(command.Amount);

        var debitMovement = new Movement(sourceWallet.Id, command.Amount, MovementType.Débito);
        var creditMovement = new Movement(destinationWallet.Id, command.Amount, MovementType.Crédito);

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Wallets.UpdateAsync(sourceWallet, cancellationToken);
            await _unitOfWork.Wallets.UpdateAsync(destinationWallet, cancellationToken);
            await _unitOfWork.Movements.AddAsync(debitMovement, cancellationToken);
            await _unitOfWork.Movements.AddAsync(creditMovement, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Failure("Un error ocurrió durante la transferencia.", [ex.Message]);
        }

        return Result.Success("Transferencia completada con éxito.");
    }

    private Result Validate(TransferBalanceCommand command, Wallet sourceWallet, Wallet destinationWallet)
    {
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
