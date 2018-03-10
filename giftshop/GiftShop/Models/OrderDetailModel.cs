namespace GiftShop.Models
{
    public class OrderDetailModel
    {
        public int ID { get; set; }

        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public ProductModel Product { get; set; }
    }
}