using Microsoft.EntityFrameworkCore;
using Damda_Service.Models;

namespace Damda_Service.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<User> User { get; set; }
    }
}
