using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBondService
    {
        Task AcquireBondSnapshotsAsync(string typeOfBond);
        Task<IList<Bond>> GetBonds(string typeOfBond);
    }
}