using WalletAPI.Application.Shared;

namespace WalletAPI.Application.Wallets.Create;

public class CreateWalletCommand : ICommand<Result<int>>
{
    public string DocumentId { get; set; }
    public string Name { get; set; }
}
