using Microsoft.EntityFrameworkCore;
using Slip.Service.DAL;

namespace Slip.Service.Chain.SlipChain;

public class SlipExists : ISlipChain
{
    private readonly SlipContext _slipContext;
    private readonly ILogger<SlipExists> _logger;
    public SlipExists(SlipContext slipContext, ILogger<SlipExists> logger)
    {
        _slipContext = slipContext;
        _logger = logger;
    }
    
    public async Task<bool> Handle(Domain.Slip slip, CancellationToken cancellationToken)
    {
        var slipExists = await 
            _slipContext.Slips
                .AnyAsync(s => s.Id == slip.Id, cancellationToken);

        if (slipExists)
        {
            _logger.LogWarning("Slip {SlipId} already exists", slip.Id);
            return false;
        }
        
        return true;
    }
}