using WalletAPI.Application.Shared;

namespace WalletAPI.Application.Wallets.Delete;

public sealed record DeleteWalletCommand(int Id) : ICommand<Result>;