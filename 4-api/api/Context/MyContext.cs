using api.Model;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace api.Context{
    public class MyContext : DbContext {
        public MyContext(DbContextOptions<MyContext> options) : base(options) {

        }

        // DbSet digunakan untuk mendefiniskan tabel, jadi jumlahnya harus sesuai dengan tabel
        public DbSet<Employee> Employees {get; set;}

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Employee>()
                .HasIndex(employee => new {employee.Phone, employee.Email})
                .IsUnique();
        }
    }
}