using FluentValidation;

namespace WalletAPI.Application.Wallets.Update;

public class UpdateWalletCommandValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .Length(10).WithMessage("El documentoID debe tener exactamente 10 dígitos.")
            .Matches(@"^\d{10}$").WithMessage("El documentoID debe contener solo números.")
            .When(x => !string.IsNullOrWhiteSpace(x.DocumentId));

        RuleFor(x => x.Name)
            .MaximumLength(200).WithMessage("El nombre no puede exceder de 200 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0).WithMessage("El balance debe ser mayor o igual a cero.")
            .When(x => x.Balance.HasValue);
    }
}