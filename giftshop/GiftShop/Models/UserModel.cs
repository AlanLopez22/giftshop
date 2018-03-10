namespace GiftShop.Models
{
    using System;
    
    public class UserModel
    {
        public int ID { get; set; }

        public int UserTypeID { get; set; }
        
        public string UserName { get; set; }
        
        public string Password { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        public string Street { get; set; }
        
        public string StreetNumber { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string Country { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
        
        public UserTypeModel UserType { get; set; }
    }
}