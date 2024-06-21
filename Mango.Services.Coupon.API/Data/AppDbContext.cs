using Mango.Services.Coupon.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Coupon.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Coupons> Coupons { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Coupons>().HasData(
                new Coupons
                {
                    CouponId = 1,
                    CouponCode = "10OFF",
                    DiscountAmount = 10,
                    MinAmount = 20
                });

            modelBuilder.Entity<Coupons>().HasData(
               new Coupons
               {
                   CouponId = 2,
                   CouponCode = "20OFF",
                   DiscountAmount = 20,
                   MinAmount = 20
               });
        }


    }
}
