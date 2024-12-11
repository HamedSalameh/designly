namespace Projects.Application.Features.SearchProperties
{
    public sealed class SearchPropertiesRequest
    {
        public Guid? Id { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
    }
}
