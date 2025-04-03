using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Api.Entities
{
    [Table("product")]
    public sealed class Products
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("subcategory_id")]
        public Guid Subcategory { get; set; }
        [Column("ski")]
        public string Ski { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("created_at")]
        public DateTimeOffset Createdat { get; set; }
        [Column("updated_at")]
        public DateTimeOffset Updatedat { get; set; }
    }
}
