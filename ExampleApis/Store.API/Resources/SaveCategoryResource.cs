using System.ComponentModel.DataAnnotations;

namespace Store.API.Resources
{
    public record SaveCategoryResource
    {
        [Required]
        [MaxLength(30)]
        public string? Name { get; init; }
    }
}