using System.ComponentModel.DataAnnotations;

namespace API_Tutorial.Models
{
    public class ProductVM
    {
        [StringLength(50)]
        public string ProductName { get; set; }

        
        public decimal Price { get; set; }
    }
}