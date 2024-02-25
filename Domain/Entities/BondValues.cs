using System;

namespace Domain.Entities;

/// <summary>
/// Represents the values associated with a bond at a specific time.
/// </summary>
public class BondValues
{
    /// <summary>
    /// The price of the bond.
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// The yield of the bond.
    /// </summary>
    public double Yield { get; set; }

    /// <summary>
    /// The UTC timestamp associated with the bond values.
    /// </summary>
    public DateTime UtcTime { get; }

    /// <summary>
    /// Default constructor for the BondValues class.
    /// </summary>
    public BondValues()
    {
        // Default constructor intentionally left empty.
    }

    /// <summary>
    /// Initializes a new instance of the BondValues class with specified parameters.
    /// </summary>
    /// <param name="price">The price of the bond.</param>
    /// <param name="annualizedGrossYield">The yield of the bond.</param>
    /// <param name="dateTime">The UTC timestamp associated with the bond values.</param>
    public BondValues(double price, double annualizedGrossYield, DateTime dateTime)
    {
        Price = price;
        Yield = annualizedGrossYield;
        UtcTime = dateTime;
    }
}
