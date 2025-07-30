using WalletAPI.Application.Interfaces;
using WalletAPI.Application.Shared;
using WalletAPI.Domain.Entities;

namespace WalletAPI.Application.Wallets.Create;

public class CreateWalletCommandHandler (IWalletRepository _walletRepository): ICommandHandler<CreateWalletCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateWalletCommand command, CancellationToken cancellationToken)
    {
        var existingWallet = await _walletRepository.GetByDocumentIdAsync(command.DocumentId, cancellationToken);
        if (existingWallet != null)
        {
            return Result.Failure<int>($"Ya existe una billetera con el documentoID {command.DocumentId}.", new[] {"El documentoID debe ser único."});
        }

        var wallet = new Wallet(command.DocumentId, command.Name);

        await _walletRepository.AddAsync(wallet, cancellationToken);

        return Result.Success(wallet.Id, "Billetera creada con éxito.");
    }
}
