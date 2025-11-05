namespace Shared.Events;

//public record GrpcCallProcessed(Guid Id, DateTime ProcessedAt) : IGrpcCallProcessed;

public record GrpcCallProcessed(Guid Id, DateTime ProcessedAt);