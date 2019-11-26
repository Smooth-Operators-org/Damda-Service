using Microsoft.EntityFrameworkCore;
using Damda_Service.Models;

namespace Damda_Service.Data
{
    public class CouponContext : DbContext
    {
        public CouponContext(DbContextOptions<CouponContext> options) : base(options) {}
        public DbSet<Coupon> Coupon { get; set; }
    }
}
