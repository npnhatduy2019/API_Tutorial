namespace API_Tutorial.Models
{
    public class OrderDetail
    {
        public int ProductId { get; set; }

        public Guid OrderId { get; set; }

        public int Qty { get; set; }

        public decimal Price { get; set; }

        public byte IsDiscount { get; set; }

        //relationship
        public OrderModel order{get;set;}
        public ProductModel product{get;set;}
    }
}