namespace Domain.Entities;

public class Bond
{
    public string Isin { get; set; }
    public string Name { get; set; }
    public double? YearlyCoupon { get; set; }
    public DateTime Expiration { get; set; }
    public string Url { get; set; }
    public string BondType { get; set; }
    public List<BondValues> HistoricalValues { get; set; }

    public Bond()
    {
        HistoricalValues = new List<BondValues>();
    }

    public Bond(string isin,
                string name,
                double? cedolaAnnuale,
                DateTime scadenza,
                string detailsUrl,
                string bondType)
    {
        Isin = isin;
        Name = name;
        YearlyCoupon = cedolaAnnuale;
        Expiration = scadenza;
        Url = detailsUrl;
        HistoricalValues = new();
        BondType = bondType;
    }

    public void FillHystoricalData(List<BondValues> historicalValues)
    {
        this.HistoricalValues = historicalValues;
    }
}
