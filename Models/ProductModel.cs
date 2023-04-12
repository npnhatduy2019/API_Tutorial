using System.ComponentModel.DataAnnotations;

namespace API_Tutorial.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime CreateDate { get; set; }=DateTime.UtcNow;
    }
}