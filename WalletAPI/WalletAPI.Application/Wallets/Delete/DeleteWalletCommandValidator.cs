using FluentValidation;

namespace WalletAPI.Application.Wallets.Delete;

internal sealed class DeleteWalletCommandValidator : AbstractValidator<DeleteWalletCommand>
{
    public DeleteWalletCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Ingrese un id mayor a 0");
    }
}
