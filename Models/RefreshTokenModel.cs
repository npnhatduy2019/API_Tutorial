using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Tutorial.Models
{
    public class RefreshTokenModel
    {
        [Key]
        public Guid Id { get; set; }

        public int UserId{get;set;}
        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime IsSuedAt { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}