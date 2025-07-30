namespace WalletAPI.Application.DTOs;

public class MovementsDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; }

    public WalletInfoDto Wallet { get; set; }
}

public class WalletInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DocumentId { get; set; }
}
