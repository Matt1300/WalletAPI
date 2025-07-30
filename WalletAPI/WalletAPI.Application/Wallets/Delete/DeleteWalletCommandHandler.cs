using WalletAPI.Application.Shared;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Repositories;

namespace WalletAPI.Application.Wallets.Delete;

public class DeleteWalletCommandHandler(IUnitOfWork _unitOfWork) : ICommandHandler<DeleteWalletCommand, Result>
{
    public async Task<Result> Handle(DeleteWalletCommand command, CancellationToken cancellationToken)
    {
        Wallet? wallet = await _unitOfWork.Wallets.GetByIdAsync(command.Id, cancellationToken);
        if (wallet is null)
        {
            return Result.Failure($"Ingrese una billetera válida para poder eliminarla", [$"No se encontró una billetera con id {command.Id}"]);
        }

        await _unitOfWork.Wallets.DeleteAsync(wallet, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success($"Billetera con id {command.Id} eliminada correctamente.");
    }
}
