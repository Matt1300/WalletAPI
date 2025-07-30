using WalletAPI.Application.Shared;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Repositories;

namespace WalletAPI.Application.Wallets.Update;

public class UpdateWalletCommandHandler(IUnitOfWork _unitOfWork) : ICommandHandler<UpdateWalletCommand, Result>
{
    public async Task<Result> Handle(UpdateWalletCommand command, CancellationToken cancellationToken)
    {
        Wallet? wallet = await _unitOfWork.Wallets.GetByIdAsync(command.Id, cancellationToken);
        if (wallet is null)
        {
            return Result.Failure("Billetera no encontrada.", [$"No existe una billetera relacionada con el Id: {command.Id}"]);
        }

        wallet.Update(command.DocumentId, command.Name, command.Balance);
        await _unitOfWork.Wallets.UpdateAsync(wallet, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
