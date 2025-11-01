namespace Store.API.Resources
{
    public record QueryResource
    {
        public required int Page { get; init; }
        public required int ItemsPerPage { get; init; }
    }
}