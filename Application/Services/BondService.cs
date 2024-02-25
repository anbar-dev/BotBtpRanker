using Application.Interfaces;
using Domain.Entities;
using AutoMapper;

namespace Application.Services;
/// <summary>
/// Represents a service for managing bonds.
/// </summary>
public class BondService : IBondService
{
    private readonly IBondRepository _bondRepository;
    private readonly IBondFetcher _dataFetcher;
    public BondService(IBondRepository bondRepository, IBondFetcher dataFetcher)
    {
        _bondRepository = bondRepository;
        _dataFetcher = dataFetcher;
    }

    /// <inheritdoc />
    public async Task AcquireBondSnapshotsAsync(string typeOfBond)
    {
        // fetch the bond data
        IEnumerable<BondSnapshot> rawBondsnapshots = await _dataFetcher.FetchBondsAsync(typeOfBond);

        // filter out expired bond data
        List<BondSnapshot> bondsnapshots = new();
        foreach (var rawBondSnapshot in rawBondsnapshots)
        {
            if (rawBondSnapshot.Expiration > DateTime.UtcNow)
            {
                bondsnapshots.Add(rawBondSnapshot);
            }
        }

        // store the bond snapshots in the database
        _bondRepository.StoreBondSnapshots(bondsnapshots, typeOfBond);
    }
    /// <inheritdoc />
    public async Task<IList<Bond>> GetBonds(string typeOfBond)
    {
        IList<Bond> bonds = await _bondRepository.GetActiveBondsAsync(typeOfBond);

        return bonds;
    }
}
