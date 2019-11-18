using Microsoft.EntityFrameworkCore;
using Damda_Service.Models;

namespace Damda_Service.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<User> User { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupSettings> GroupSettings { get; set; }

    }
}
