using Event.Core.DomainEvents.MarketTemplate;
using Event.Core.Repositories;
using Shared.Cqrs.Commands;

namespace Event.Application.MarketTemplate.Commands.CreateNewMarketTemplate;

public class CreateMarketTemplateCommandHandler : ICommandHandler<CreateMarketTemplateCommand>
{
    private readonly IMarketTemplateRepository _repository;
    public CreateMarketTemplateCommandHandler(IMarketTemplateRepository repository)
    {
        _repository = repository;
    }
    public async Task HandleAsync(CreateMarketTemplateCommand command, CancellationToken cancellationToken = default)
    {
        var marketTemplate = Core.Models.MarketTemplate
            .Create(command.name, command.category)
            .WithStakeLimits(command.minStake, command.maxStake);
        
        marketTemplate.AddDomainEvent(new MarketTemplateCreated());
        
        await _repository.Add(marketTemplate);
        
    }
}