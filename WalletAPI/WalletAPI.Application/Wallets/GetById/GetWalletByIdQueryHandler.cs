using WalletAPI.Application.DTOs;
using WalletAPI.Application.Interfaces;
using WalletAPI.Application.Shared;

namespace WalletAPI.Application.Wallets.GetById;

public class GetWalletByIdQueryHandler(IWalletRepository _walletRepository) : IQueryHandler<GetWalletByIdQuery, Result<WalletDto>>
{
    public async Task<Result<WalletDto>> Handle(GetWalletByIdQuery query, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(query.Id, cancellationToken);
        if (wallet == null)
        {
            return Result.Failure<WalletDto>($"No se encontró una billetera con el ID {query.Id}.", ["El ID de la billetera no existe."]);
        }

        var walletDto = new WalletDto
        {
            Id = wallet.Id,
            DocumentId = wallet.DocumentId,
            Name = wallet.Name,
            Balance = wallet.Balance,
            CreatedAt = wallet.CreatedAt,
            UpdatedAt = wallet.UpdatedAt
        };

        return Result.Success(walletDto, "Billetera obtenida con éxito.");
    }
}
