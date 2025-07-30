using WalletAPI.Application.Shared;

namespace WalletAPI.Application.Wallets.Update;

public sealed record UpdateWalletCommand(
    int Id,
    string? DocumentId,
    string? Name,
    decimal? Balance) : ICommand<Result>;
