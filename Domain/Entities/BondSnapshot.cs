using System;

namespace Domain.Entities;

/// <summary>
/// Represents a snapshot of bond data at a specific time.
/// </summary>
public class BondSnapshot
{
    /// <summary>
    /// The UTC timestamp of the snapshot.
    /// </summary>
    public DateTime UtcTime { get; set; }

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
    public double? YearlyCoupon { get; }

    /// <summary>
    /// The expiration date of the bond.
    /// </summary>
    public DateTime Expiration { get; }

    /// <summary>
    /// The URL providing details about the bond.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// The price of the bond.
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// The yield of the bond.
    /// </summary>
    public double Yield { get; set; }

    /// <summary>
    /// The type of the bond.
    /// </summary>
    public string BondType { get; set; }

    /// <summary>
    /// Initializes a new instance of the BondSnapshot class with specified parameters.
    /// </summary>
    /// <param name="snapshotDateTime">The UTC timestamp of the snapshot.</param>
    /// <param name="isin">The ISIN of the bond.</param>
    /// <param name="name">The name of the bond.</param>
    /// <param name="yearlyCoupon">The yearly coupon rate of the bond.</param>
    /// <param name="expiration">The expiration date of the bond.</param>
    /// <param name="detailsUrl">The URL providing details about the bond.</param>
    /// <param name="bondType">The type of the bond.</param>
    /// <param name="price">The price of the bond.</param>
    /// <param name="yield">The yield of the bond.</param>
    public BondSnapshot(DateTime snapshotDateTime, string isin, string name, double? yearlyCoupon,
                        DateTime expiration, string detailsUrl, string bondType, double price, double yield)
    {
        UtcTime = snapshotDateTime;
        Isin = isin;
        Name = name;
        YearlyCoupon = yearlyCoupon;
        Expiration = expiration;
        Url = detailsUrl;
        BondType = bondType;
        Price = price;
        Yield = yield;
    }
}
