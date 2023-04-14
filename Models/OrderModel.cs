using System.ComponentModel.DataAnnotations;

namespace API_Tutorial.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime?  DateTransfer{ get; set; }

        public string CustName{get;set;}

        public string Address{get;set;}
        public  string Phone{get;set;}

        public OrderStatus Status { get; set; }

        public ICollection<OrderDetail> orderDetails{get;set;}

        public OrderModel()
        {
            orderDetails=new List<OrderDetail>();
        }
    }
    public enum OrderStatus{
        New = 0,
        Payment=1,

        Complete=2,

        cancle=-1
    }
}
