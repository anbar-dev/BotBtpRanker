using System;
using System.Collections.Generic;

namespace Domain.Entities;

/// <summary>
/// Represents a bond entity.
/// </summary>
public class Bond
{
    /// <summary>
    /// The International Securities Identification Number (ISIN) of the bond.
    /// </summary>
    public string Isin { get; set; }

    /// <summary>
    /// The name of the bond.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The yearly coupon rate of the bond.
    /// </summary>
    public double? YearlyCoupon { get; set; }

    /// <summary>
    /// The expiration date of the bond.
    /// </summary>
    public DateTime Expiration { get; set; }

    /// <summary>
    /// The URL providing details about the bond.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// The type of the bond.
    /// </summary>
    public string BondType { get; set; }

    /// <summary>
    /// The historical values associated with the bond.
    /// </summary>
    public List<BondValues> HistoricalValues { get; set; }

    /// <summary>
    /// Default constructor for the Bond class.
    /// </summary>
    public Bond()
    {
        HistoricalValues = new List<BondValues>();
    }

    /// <summary>
    /// Parameterized constructor for the Bond class.
    /// </summary>
    /// <param name="isin">The ISIN of the bond.</param>
    /// <param name="name">The name of the bond.</param>
    /// <param name="yearlyCoupon">The yearly coupon rate of the bond.</param>
    /// <param name="expiration">The expiration date of the bond.</param>
    /// <param name="detailsUrl">The URL providing details about the bond.</param>
    /// <param name="bondType">The type of the bond.</param>
    public Bond(string isin, string name, double? yearlyCoupon, DateTime expiration, string detailsUrl, string bondType)
    {
        Isin = isin;
        Name = name;
        YearlyCoupon = yearlyCoupon;
        Expiration = expiration;
        Url = detailsUrl;
        BondType = bondType;
        HistoricalValues = new();
    }

    /// <summary>
    /// Fills historical data for the bond.
    /// </summary>
    /// <param name="historicalValues">The historical values to be filled.</param>
    public void FillHistoricalData(List<BondValues> historicalValues)
    {
        HistoricalValues = historicalValues;
    }
}
