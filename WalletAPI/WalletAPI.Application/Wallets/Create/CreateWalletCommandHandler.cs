using WalletAPI.Application.Shared;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Repositories;

namespace WalletAPI.Application.Wallets.Create;

public class CreateWalletCommandHandler (IUnitOfWork _unitOfWork) : ICommandHandler<CreateWalletCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateWalletCommand command, CancellationToken cancellationToken)
    {
        var existingWallet = await _unitOfWork.Wallets.GetByDocumentIdAsync(command.DocumentId, cancellationToken);
        if (existingWallet != null)
        {
            return Result.Failure<int>($"Ya existe una billetera con el documentoID {command.DocumentId}.", ["El documentoID debe ser único."]);
        }

        var wallet = new Wallet(command.DocumentId, command.Name);

        await _unitOfWork.Wallets.AddAsync(wallet, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(wallet.Id, "Billetera creada con éxito.");
    }
}
