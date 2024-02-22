namespace Domain.Entities;

public class BondValues
{
    public double Price { get; set; }
    public double Yield { get; set; }
    public DateTime UtcTime { get; }
    public BondValues()
    {
        
    }

    public BondValues(double price, double annualizedGrossYield, DateTime dateTime)
    {
        Price = price;
        Yield = annualizedGrossYield;
        UtcTime = dateTime;
    }
}