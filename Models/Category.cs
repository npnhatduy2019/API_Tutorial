using System.ComponentModel.DataAnnotations;

namespace API_Tutorial.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ProductModel> listproduct{get;set;}
    }
}