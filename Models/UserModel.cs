using System.ComponentModel.DataAnnotations;

namespace API_Tutorial.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string PassWord { get; set; }

        [EmailAddress]

        public string Email { get; set; }

        [MaxLength(20)]
        public string Fname { get; set; }
        [MaxLength(20)]
        public string Lname{get;set;}
    }
}