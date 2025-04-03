using Products.Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace Products.Api.Contracts;

public class UpdateSubCategoryRequest
{

    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Code { get; set; }

    public Guid CategoryId { get; set; }

    [Required]
    public string Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset Updatedat { get; set; }

}
