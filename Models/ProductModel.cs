using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }
        public Category category{get;set;}

        public ICollection<OrderDetail> orderDetails{get;set;}

        public ProductModel()
        {
            orderDetails=new List<OrderDetail>();
        }
    }
}