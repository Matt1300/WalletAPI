using FluentValidation;

namespace WalletAPI.Application.Movements.Create;

public class TransferBalanceCommandValidator: AbstractValidator<TransferBalanceCommand>
{
    public TransferBalanceCommandValidator()
    {
        RuleFor(x => x.SourceWalletId)
            .GreaterThan(0).WithMessage("El id de la billetera origen debe ser mayor a 0.");

        RuleFor(x => x.DestinationWalletId)
            .GreaterThan(0).WithMessage("El id de la billetera destino debe ser mayor a 0.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto de transferencia debe ser mayo a 0.");
    }
}
