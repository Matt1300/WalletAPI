namespace WalletAPI.Domain.Entities;
public enum MovementType
{
    Crédito = 1,
    Débito
}
public class Movement
{
    public int Id { get; private set; }
    public int WalletId { get; private set; }
    public decimal Amount { get; private set; }
    public MovementType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Wallet Wallet { get; private set; } 

    private Movement() { }

    public Movement(int walletId, decimal amount, MovementType type)
    {      
        WalletId = walletId;
        Amount = amount;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }
}
