namespace GiftShop.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OrderDetail")]
    public partial class OrderDetail : IEntityBase
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}