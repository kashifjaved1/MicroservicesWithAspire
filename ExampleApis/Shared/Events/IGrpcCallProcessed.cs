namespace Shared.Events;

public interface IGrpcCallProcessed
{
    Guid Id { get; }
    DateTime ProcessedAt { get; }
}
