using BuildingBlocks.Abstractions.Persistence;
using MediatR;

namespace BuildingBlocks.Abstractions.CQRS.Commands;

public interface ITxCommand : ITxCommand<Unit> { }

public interface ITxCommand<out T> : ICommand<T>, ITxRequest
    where T : notnull { }
