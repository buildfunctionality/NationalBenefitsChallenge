using Products.Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace Products.Api.Contracts;

public class UpdateProductRequest
{
   
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }

    public Guid SubCategory { get; set; }
    [Required]
    public string Ski { get; set; }
    [Required]
    public string Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset Updatedat { get; set; }

}