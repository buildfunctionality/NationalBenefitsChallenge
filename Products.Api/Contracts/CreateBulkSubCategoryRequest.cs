using Products.Api.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Products.Api.Contracts;

public class CreateBulkSubCategoryRequest
{
   
    [Required]
    public string Code { get; set; }

    public Guid CategoryId { get; set; }
   
    [Required]
    public string Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset Updatedat { get; set; }

}

