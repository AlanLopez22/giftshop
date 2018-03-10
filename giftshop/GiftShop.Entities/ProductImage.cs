namespace GiftShop.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductImage")]
    public partial class ProductImage : IEntityBase
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        [Required]
        [StringLength(500)]
        public string ImagePath { get; set; }

        public virtual Product Product { get; set; }
    }
}