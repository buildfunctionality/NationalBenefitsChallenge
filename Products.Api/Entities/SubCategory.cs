using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Api.Entities
{
    [Table("subcategory")]
    public class SubCategory
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("description")]
        public string Description { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }
        [Column("created_at")]
        public DateTimeOffset Created_at { get; set; }
        [Column("updated_at")]

        public DateTimeOffset Updated_at { get; set; }
    }
}
