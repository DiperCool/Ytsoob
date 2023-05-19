namespace BuildingBlocks.Abstractions.Domain;

public interface IHaveCreator
{
    DateTime Created { get; }
    Guid? CreatedBy { get; }
}
