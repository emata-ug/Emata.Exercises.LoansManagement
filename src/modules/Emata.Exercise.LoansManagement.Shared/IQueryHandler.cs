using Emata.Exercise.LoansManagement.Contracts.Shared;

namespace Emata.Exercise.LoansManagement.Shared;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken = default);
}