namespace Infrastructure.Repository.Models;

public class BondDbModel
{
    public int? BondId { get; set; }
    public string Isin { get; set; }
    public string Name { get; set; }
    public double? YearlyCoupon { get; set; }
    public DateTime Expiration { get; set; }
    public string Url { get; set; }
    public string BondType { get; set; }
    public bool IsActive { get; set; } = true;
    public List<BondValuesDbModel> HistoricalValues { get; set; }
}
