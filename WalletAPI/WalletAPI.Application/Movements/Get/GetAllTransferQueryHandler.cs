using WalletAPI.Application.DTOs;
using WalletAPI.Application.Interfaces;
using WalletAPI.Application.Shared;

namespace WalletAPI.Application.Movements.Get;

public class GetAllTransferQueryHandler(IMovementRepository _movementRepository) : IQueryHandler<GetAllTransferQuery, List<MovementsDto>>
{
    public async Task<List<MovementsDto>> Handle(GetAllTransferQuery query, CancellationToken cancellationToken)
    {
        var movements = await _movementRepository.GetAllTransfersAsync(cancellationToken);

        if (movements == null || !movements.Any())
        {
            return new List<MovementsDto>();
        }

        var movementsDto = movements.Select(m => new MovementsDto
        {
            Id = m.Id,
            Amount = m.Amount,
            Type = m.Type.ToString(),
            CreatedAt = m.CreatedAt,
            Wallet = new WalletInfoDto
            {
                Id = m.Wallet.Id,
                Name = m.Wallet.Name,
                DocumentId = m.Wallet.DocumentId
            }
        }).ToList();

        return movementsDto;
    }
}
