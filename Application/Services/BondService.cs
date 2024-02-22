using Application.Interfaces;
using Domain.Entities;
using AutoMapper;

namespace Application.Services;

public class BondService : IBondService
{
    private readonly IBondRepository _bondRepository;
    private readonly IBondFetcher _dataFetcher;
    private readonly IMapper _mapper;

    public BondService(IBondRepository bondRepository, IBondFetcher dataFetcher, IMapper mapper)
    {
        _bondRepository = bondRepository;
        _dataFetcher = dataFetcher;
        _mapper = mapper;
    }

    // method to be called periodically to update the database
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

    public async Task<IList<Bond>> GetBonds(string typeOfBond)
    {
        IList<Bond> bonds = await _bondRepository.GetActiveBondsAsync(typeOfBond);

        return bonds;
    }
}
