using WalletAPI.Application.Shared;

namespace WalletAPI.Application.Movements.Create;

public class TransferBalanceCommand : ICommand<Result>
{
    public int SourceWalletId { get; set; }
    public int DestinationWalletId { get; set; }
    public decimal Amount { get; set; }
}
