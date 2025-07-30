using Moq;
using WalletAPI.Application.Wallets.Create;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Repositories;

namespace WalletAPI.UnitTests.Wallets;

public class CreateWalletCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenWalletExists()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.Wallets.GetByDocumentIdAsync("0604848445", default))
            .ReturnsAsync(new Wallet("0604848445", "Test"));

        var handler = new CreateWalletCommandHandler(mockUnitOfWork.Object);
        var command = new CreateWalletCommand { DocumentId = "0604848445", Name = "Test" };

        var result = await handler.Handle(command, default);

        Assert.False(result.Succeeded);
        Assert.Contains("único", result.Errors[0]);
    }

    [Fact]
    public async Task Handle_ShouldCreateWallet_WhenWalletDoesNotExist()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.Wallets.GetByDocumentIdAsync("1234567890", default))
            .ReturnsAsync((Wallet?)null);

        mockUnitOfWork.Setup(u => u.Wallets.AddAsync(It.IsAny<Wallet>(), default))
            .Returns(Task.CompletedTask);

        mockUnitOfWork.Setup(u => u.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var handler = new CreateWalletCommandHandler(mockUnitOfWork.Object);
        var command = new CreateWalletCommand { DocumentId = "1234567890", Name = "Nuevo" };

        var result = await handler.Handle(command, default);

        Assert.True(result.Succeeded);
        Assert.Equal("Billetera creada con éxito.", result.Message);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.Wallets.GetByDocumentIdAsync("1234567890", default))
            .ReturnsAsync((Wallet?)null);

        mockUnitOfWork.Setup(u => u.Wallets.AddAsync(It.IsAny<Wallet>(), default))
            .ThrowsAsync(new Exception("DB error"));

        var handler = new CreateWalletCommandHandler(mockUnitOfWork.Object);
        var command = new CreateWalletCommand { DocumentId = "1234567890", Name = "Nuevo" };

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldCreateWallet_WithBoundaryValues()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.Wallets.GetByDocumentIdAsync("12345678", default))
            .ReturnsAsync((Wallet?)null);

        mockUnitOfWork.Setup(u => u.Wallets.AddAsync(It.IsAny<Wallet>(), default))
            .Returns(Task.CompletedTask);

        mockUnitOfWork.Setup(u => u.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var handler = new CreateWalletCommandHandler(mockUnitOfWork.Object);
        var command = new CreateWalletCommand { DocumentId = "12345678", Name = new string('A', 100) };

        var result = await handler.Handle(command, default);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validator_ShouldFail_WhenDocumentIdIsEmpty()
    {
        var validator = new CreateWalletCommandValidator();
        var result = validator.Validate(new CreateWalletCommand { DocumentId = "", Name = "Test" });
        Assert.False(result.IsValid);
    }
}
