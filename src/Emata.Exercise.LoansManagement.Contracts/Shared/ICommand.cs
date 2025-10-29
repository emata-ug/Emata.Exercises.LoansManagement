namespace Emata.Exercise.LoansManagement.Contracts.Shared;

// Marker command interface. TResponse is covariant allowing broader assignment.
public interface ICommand<out TResponse> { }