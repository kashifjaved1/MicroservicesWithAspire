namespace Store.API.Resources
{
    public record ProductsQueryResource : QueryResource
    {
        public int? CategoryId { get; init; }
    }
}