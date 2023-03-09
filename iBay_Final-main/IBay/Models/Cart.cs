using System;
namespace IBay.Models
{
    public class Cart
    {
        public Cart()
        {
            Products = new List<Product>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public List<Product> Products { get; set; }
    }
}

