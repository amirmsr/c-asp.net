using System;
using System.ComponentModel.DataAnnotations;

namespace IBay.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Available is required.")]
        public bool Available { get; set; }

        [Required(ErrorMessage = "AddedTime is required.")]
        public DateTime AddedTime { get; set; }

        public int SellerId { get; set; }
    }
}

