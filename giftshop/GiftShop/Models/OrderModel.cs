namespace GiftShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public class OrderModel
    {
        public OrderModel()
        {
            Details = new List<OrderDetailModel>();
        }

        public int ID { get; set; }

        public int? UserID { get; set; }

        public int StatusID { get; set; }

        public float Amount { get; set; }

        public DateTime CreatedOn { get; set; }

        public StatusModel Status { get; set; }

        public UserModel User { get; set; }

        public List<OrderDetailModel> Details { get; set; }
    }
}