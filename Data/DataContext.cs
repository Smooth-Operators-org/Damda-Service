using Microsoft.EntityFrameworkCore;
using Damda_Service.Models;

namespace Damda_Service.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<User> User { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<GroupSettings> GroupSettings { get; set; }
        public DbSet<GroupHasUsers> GroupHasUsers { get; set; }

    }
}
