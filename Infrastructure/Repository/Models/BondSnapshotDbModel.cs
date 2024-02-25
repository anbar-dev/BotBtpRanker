namespace Infrastructure.Repository.Models;

public class BondSnapshotDbModel
{
    public int? BondId { get; set; }
    public DateTime UtcTime { get; set; }
    public string Isin { get; set; }
    public string Name { get; set; }
    public double? YearlyCoupon { get; set; }
    public DateTime Expiration { get; set; }
    public string Url { get; set; }
    public double Price { get; set; }
    public double Yield { get; set; }
    public string BondType { get; set; }
    public bool IsActive { get; set; } = true;
}
