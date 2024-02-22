namespace Domain.Entities;

public class BondSnapshot
{
    public DateTime UtcTime { get; set; }
    public string Isin { get; set; }
    public string Name { get; set; }
    public double? YearlyCoupon { get; }
    public DateTime Expiration { get; }
    public string Url { get; set; }
    public double Price { get; set; }
    public double Yield { get; set; }
    public string BondType { get; set; }

    public BondSnapshot(DateTime snapshotDateTime,
                string isin,
                string name,
                double? cedolaAnnuale,
                DateTime scadenza,
                string detailsUrl,
                string bondType,
                double price,
                double yield)
    {
        UtcTime = snapshotDateTime;
        Isin = isin;
        Name = name;
        YearlyCoupon = cedolaAnnuale;
        Expiration = scadenza;
        Url = detailsUrl;
        BondType = bondType;
        Price = price;
        Yield = yield;
    }
}
