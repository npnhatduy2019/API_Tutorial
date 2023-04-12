using System.ComponentModel.DataAnnotations;

namespace API_Tutorial.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Tiêu đề")]
        [StringLength(100,ErrorMessage ="{0} phải từ {1} đến {2} ký tự",MinimumLength =3)]
        public string Title { get; set; }

        [Display(Name ="Nội dung")]
        [StringLength(1000,ErrorMessage ="{0} phải từ {1} đến {2} ký tự",MinimumLength =3)]
        public string Description { get; set; }

        [Display(Name ="Ngày tạo")]
        [Required]
        public DateTime DateCreate{get;set;}
    }
}