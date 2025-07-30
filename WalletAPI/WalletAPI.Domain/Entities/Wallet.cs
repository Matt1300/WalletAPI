namespace WalletAPI.Domain.Entities;
public class Wallet
{
    public int Id { get; private set; }
    public string DocumentId { get; private set; }
    public string Name { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public ICollection<Movement> Movements { get; private set; } 

    private Wallet() { }

    public Wallet(string documentId, string name)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("El documentoID no puede ser nulo o vacío.", nameof(documentId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede ser nulo o vacío.", nameof(name));

        DocumentId = documentId;
        Name = name;
        Balance = 0;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Movements = new List<Movement>();
    }

    public void Credit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(amount));

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Debit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(amount));
        if (Balance < amount)
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;
    }
}
