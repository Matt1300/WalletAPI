using Moq;
using WalletAPI.Application.Movements.Create;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Repositories;

namespace WalletAPI.UnitTests;

public class MovementsCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSourceWalletDoesNotExist()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(1, default))
            .ReturnsAsync((Wallet?)null);
        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(2, default))
            .ReturnsAsync(new Wallet("22222222", "Destino"));

        var handler = new TransferBalanceCommandHandler(mockUnitOfWork.Object);
        var command = new TransferBalanceCommand { SourceWalletId = 1, DestinationWalletId = 2, Amount = 100 };

        var result = await handler.Handle(command, default);

        Assert.False(result.Succeeded);
        Assert.Contains("origen", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenDestinationWalletDoesNotExist()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(1, default))
            .ReturnsAsync(new Wallet("11111111", "Origen") { Balance = 200 });
        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(2, default))
            .ReturnsAsync((Wallet?)null);

        var handler = new TransferBalanceCommandHandler(mockUnitOfWork.Object);
        var command = new TransferBalanceCommand { SourceWalletId = 1, DestinationWalletId = 2, Amount = 100 };

        var result = await handler.Handle(command, default);

        Assert.False(result.Succeeded);
        Assert.Contains("destino", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSourceWalletHasInsufficientBalance()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var sourceWallet = new Wallet("11111111", "Origen") { Balance = 50 };
        var destinationWallet = new Wallet("22222222", "Destino") { Balance = 0 };

        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(1, default))
            .ReturnsAsync(sourceWallet);
        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(2, default))
            .ReturnsAsync(destinationWallet);

        var handler = new TransferBalanceCommandHandler(mockUnitOfWork.Object);
        var command = new TransferBalanceCommand { SourceWalletId = 1, DestinationWalletId = 2, Amount = 100 };

        var result = await handler.Handle(command, default);

        Assert.False(result.Succeeded);
        Assert.Contains("insuficiente", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenTransferIsValid()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var sourceWallet = new Wallet("11111111", "Origen") { Id = 1, Balance = 200 };
        var destinationWallet = new Wallet("22222222", "Destino") { Id = 2, Balance = 50 };

        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(1, default))
            .ReturnsAsync(sourceWallet);
        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(2, default))
            .ReturnsAsync(destinationWallet);

        mockUnitOfWork.Setup(u => u.Wallets.UpdateAsync(It.IsAny<Wallet>(), default))
            .Returns(Task.CompletedTask);
        mockUnitOfWork.Setup(u => u.Movements.AddAsync(It.IsAny<Movement>(), default))
            .Returns(Task.CompletedTask);
        mockUnitOfWork.Setup(u => u.SaveChangesAsync(default))
            .ReturnsAsync(1);
        mockUnitOfWork.Setup(u => u.BeginTransactionAsync())
            .Returns(Task.CompletedTask);
        mockUnitOfWork.Setup(u => u.CommitTransactionAsync())
            .Returns(Task.CompletedTask);

        var handler = new TransferBalanceCommandHandler(mockUnitOfWork.Object);
        var command = new TransferBalanceCommand { SourceWalletId = 1, DestinationWalletId = 2, Amount = 100 };

        var result = await handler.Handle(command, default);

        Assert.True(result.Succeeded);
        Assert.Equal("Transferencia completada con éxito.", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldRollbackTransaction_WhenExceptionOccurs()
    {
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var sourceWallet = new Wallet("11111111", "Origen") { Id = 1, Balance = 200 };
        var destinationWallet = new Wallet("22222222", "Destino") { Id = 2, Balance = 50 };

        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(1, default))
            .ReturnsAsync(sourceWallet);
        mockUnitOfWork.Setup(u => u.Wallets.GetByIdAsync(2, default))
            .ReturnsAsync(destinationWallet);

        mockUnitOfWork.Setup(u => u.Wallets.UpdateAsync(It.IsAny<Wallet>(), default))
            .ThrowsAsync(new Exception("DB error"));
        mockUnitOfWork.Setup(u => u.RollbackTransactionAsync())
            .Returns(Task.CompletedTask);

        var handler = new TransferBalanceCommandHandler(mockUnitOfWork.Object);
        var command = new TransferBalanceCommand { SourceWalletId = 1, DestinationWalletId = 2, Amount = 100 };

        var result = await handler.Handle(command, default);

        Assert.False(result.Succeeded);
        Assert.Contains("error", result.Message);
        Assert.Contains("DB error", result.Errors[0]);
    }
}
