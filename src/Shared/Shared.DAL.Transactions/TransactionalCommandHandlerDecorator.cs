using Shared.Cqrs.Commands;
using Shared.DAL.Postgres;
using Shared.Types.Attributes;

namespace Shared.DAL.Transactions;

[Decorator]
internal sealed class TransactionalCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
{
    private readonly ICommandHandler<T> _handler;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionalCommandHandlerDecorator(ICommandHandler<T> handler, IUnitOfWork unitOfWork)
    {
        _handler = handler;
        _unitOfWork = unitOfWork;
    }

    public Task HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        return _unitOfWork.ExecuteAsync(() => _handler.HandleAsync(command, cancellationToken), cancellationToken);
    }
}