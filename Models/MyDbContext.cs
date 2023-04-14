using Microsoft.EntityFrameworkCore;

namespace API_Tutorial.Models
{
     public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderModel>(e=>{
                e.ToTable("Orders");
                e.HasKey(e=>e.Id);
                e.HasIndex(e=>e.Id);
                e.Property(p=>p.DateCreate).HasDefaultValue(DateTime.UtcNow);     
                e.Property(p=>p.CustName).IsRequired().HasMaxLength(50);
                e.Property(p=>p.Phone).HasAnnotation("RegularExpression", @"\d{10,11}").HasMaxLength(20);           
            }            
            );

            builder.Entity<OrderDetail>(e=>{
                e.ToTable("OrderDetails");
                e.HasKey(e=>new {e.OrderId,e.ProductId});
                e.HasIndex(e=>new {e.ProductId,e.OrderId});
                e.HasOne(o=>o.order).WithMany(od=>od.orderDetails).HasForeignKey(e=>e.OrderId).HasConstraintName("FK_OrderDetail_Order");
                e.HasOne(e=>e.product).WithMany(p=>p.orderDetails).HasForeignKey(e=>e.ProductId).HasConstraintName("FK_OrderDetail_Product");;
            });
        }

        //public DbSet<Article> Articles{get;set;}
        public DbSet<ProductModel>  Products{get;set;}
        public DbSet<Category> categories{get;set;}

        public DbSet<OrderModel> orderModels{get;set;}
        public DbSet<OrderDetail> orderDetails{get;set;}

    }
}