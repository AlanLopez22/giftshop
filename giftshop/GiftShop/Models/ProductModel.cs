namespace GiftShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public class ProductModel
    {
        public ProductModel()
        {
            Images = new List<ProductImageModel>();
        }

        public int ID { get; set; }

        public int CategoryID { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public float Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public CategoryModel Category { get; set; }
        
        public List<ProductImageModel> Images { get; set; }
    }
}