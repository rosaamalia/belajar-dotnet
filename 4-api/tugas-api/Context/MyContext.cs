using tugas_api.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace tugas_api.Context{
    public class MyContext : DbContext {
        public MyContext(DbContextOptions<MyContext> options) : base(options) {

        }

        public DbSet<Employee> Employees {get; set;}
        public DbSet<Department> Departments {get; set;}

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Department>()
                .HasMany(e => e.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.Department_id)
                .IsRequired();
        }
    }
}