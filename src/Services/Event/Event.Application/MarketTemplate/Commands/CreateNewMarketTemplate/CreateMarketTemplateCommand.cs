using Shared.Cqrs.Commands;

namespace Event.Application.MarketTemplate.Commands.CreateNewMarketTemplate;

public record CreateMarketTemplateCommand(string name, string category, decimal minStake, decimal maxStake) : ICommand;
