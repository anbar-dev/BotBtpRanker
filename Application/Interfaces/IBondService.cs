using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Represents a service for managing bonds.
/// </summary>
public interface IBondService
{
    /// <summary>
    /// Asynchronously acquires bond snapshots of the specified type and stores them in the database.
    /// </summary>
    /// <param name="typeOfBond">The type of bond for which snapshots are to be acquired.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AcquireBondSnapshotsAsync(string typeOfBond);

    /// <summary>
    /// Asynchronously retrieves active bonds of the specified type from the database.
    /// </summary>
    /// <param name="typeOfBond">The type of bond to retrieve.</param>
    /// <returns>A list of active bonds.</returns>
    Task<IList<Bond>> GetBonds(string typeOfBond);
}
