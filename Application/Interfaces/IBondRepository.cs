using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Represents a service for storing bonds.
/// </summary>
public interface IBondRepository
{
    /// <summary>
    /// Retrieves active bonds asynchronously based on the bond type.
    /// </summary>
    /// <param name="typeOfBond">Type of bond.</param>
    /// <returns>List of active bonds.</returns>
    Task<IList<Bond>> GetActiveBondsAsync(string typeOfBond);
    /// <summary>
    /// Creates the database if it doesn't exist.
    /// </summary>
    void CreateDatabaseIfNotExists();
    /// <summary>
    /// Creates tables if they don't exist.
    /// </summary>
    void CreateTablesIfNotExists();
    /// <summary>
    /// Stores bond snapshots.
    /// </summary>
    /// <param name="bondSnapshots">Bond snapshots to store.</param>
    /// <param name="typeOfBond">Type of bond.</param>
    void StoreBondSnapshots(IEnumerable<BondSnapshot> bondSnapshotDbDtos, string typeOfBond);
}
