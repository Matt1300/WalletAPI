using FluentValidation;

namespace WalletAPI.Application.Wallets.Create;

public class CreateWalletCommandValidator: AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty().WithMessage("El documentoID es requerido.")
            .Length(10).WithMessage("El documentoID debe tener exactamente 10 dígitos.")
            .Matches(@"^\d{10}$").WithMessage("El documentoID debe contener solo números.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder de 200 caracteres.");
    }
}
