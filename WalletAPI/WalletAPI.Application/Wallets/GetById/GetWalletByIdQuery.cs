using WalletAPI.Application.DTOs;
using WalletAPI.Application.Shared;

namespace WalletAPI.Application.Wallets.GetById;

public class GetWalletByIdQuery : IQuery<Result<WalletDto>>
{
    public int Id { get; set; }

}
