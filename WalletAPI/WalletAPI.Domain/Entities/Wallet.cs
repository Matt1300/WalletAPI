namespace WalletAPI.Domain.Entities;
public class Wallet
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<Movement> Movements { get; set; } 

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

    public void Update(string? documentId, string? name, decimal? balance)
    {
        if (!string.IsNullOrWhiteSpace(documentId))
            DocumentId = documentId;

        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        if (balance.HasValue)
            Balance = balance.Value;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Credit(decimal amount)
    {
        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Debit(decimal amount)
    {
        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;
    }
}
