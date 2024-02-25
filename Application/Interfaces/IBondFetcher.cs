using Domain.Entities;

namespace Application.Interfaces;
/// <summary>
/// Represents a service for fetching bonds.
/// </summary>
public interface IBondFetcher
{
    /// <summary>
    /// Fetches bond snapshots asynchronously based on the specified type of bond.
    /// </summary>
    /// <param name="typeOfBond">The type of bond to fetch.</param>
    /// <returns>A collection of bond snapshots.</returns>
    Task<IEnumerable<BondSnapshot>> FetchBondsAsync(string typeOfBond);
}
