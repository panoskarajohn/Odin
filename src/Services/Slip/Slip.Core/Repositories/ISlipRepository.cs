namespace Slip.Core.Repositories;

public interface ISlipRepository
{
    Task <Models.Slip> GetSlipAsync(string userId, CancellationToken cancellationToken);
    Task UpdateSlipAsync(Core.Models.Slip slip, CancellationToken cancellationToken);
    Task<bool> DeleteSlipAsync(string userId, CancellationToken cancellationToken);
}