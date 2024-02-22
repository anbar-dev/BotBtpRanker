using Domain.Entities;

namespace Application.Interfaces;

public interface IBondFetcher
{
    Task<IEnumerable<BondSnapshot>> FetchBondsAsync(string typeOfBond);
}
