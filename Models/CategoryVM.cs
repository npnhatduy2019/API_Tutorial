using System.ComponentModel.DataAnnotations;

namespace API_Tutorial.Models
{
    public class CategoryVM
    {
        public int Id{get;set;}
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
    }
}