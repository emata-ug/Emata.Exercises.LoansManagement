using Emata.Exercise.LoansManagement.Contracts.Shared;

namespace Emata.Exercise.LoansManagement.Shared;

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
}