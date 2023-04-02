namespace Slip.Service.Chain.SlipChain;

public interface ISlipChain
{
    Task<bool> Handle(Domain.Slip slip, CancellationToken cancellationToken);
}