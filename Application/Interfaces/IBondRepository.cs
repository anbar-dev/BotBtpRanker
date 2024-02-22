using Domain.Entities;

namespace Application.Interfaces;

public interface IBondRepository
{
    Task<IList<Bond>> GetActiveBondsAsync(string typeOfBond);
    void CreateDatabaseIfNotExists();
    void CreateTablesIfNotExists();
    void StoreBondSnapshots(IEnumerable<BondSnapshot> bondSnapshotDbDtos, string typeOfBond);
}
