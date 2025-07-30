using FluentValidation;

namespace WalletAPI.Application.Wallets.Create;

public class CreateWalletCommandValidator: AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty().WithMessage("El documentoID es requerido.")
            .MaximumLength(10).WithMessage("El documentoID no puede exceder de 10 dígitos.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder de 200 caracteres.");
    }
}
