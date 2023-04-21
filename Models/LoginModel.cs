using System.ComponentModel.DataAnnotations;

namespace API_Tutorial.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string PassWord { get; set; }
    }
}