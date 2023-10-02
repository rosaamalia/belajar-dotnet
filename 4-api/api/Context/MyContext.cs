using api.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace api.Context{
    public class MyContext : DbContext {
        public MyContext(DbContextOptions<MyContext> options) : base(options) {

        }

        // DbSet digunakan untuk mendefiniskan tabel, jadi jumlahnya harus sesuai dengan tabel
        public DbSet<Employee> Employees {get; set;}
        public DbSet<Account> Accounts {get; set;}
        public DbSet<Profiling> Profilings {get; set;}
        public DbSet<Education> Educations {get; set;}
        public DbSet<University> Universities {get; set;}

        // protected override void OnModelCreating(ModelBuilder builder) {
        //     builder.Entity<Employee>()
        //         .HasIndex(employee => new {employee.Phone, employee.Email})
        //         .IsUnique();
        // }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Account>()
                .HasOne(a => a.Employee)
                .WithOne(e => e.Account)
                .HasForeignKey<Account>(a => a.NIK)
                .IsRequired();

            builder.Entity<Profiling>()
                .HasOne(p => p.Account)
                .WithOne(a => a.Profiling)
                .HasForeignKey<Profiling>(p => p.NIK)
                .IsRequired();

            // Kalau many-to-one, di .HasForeignKey gak perlu definisiin nama tabel
            // builder.Entity<Profiling>()
            //     .HasOne(e => e.Education)
            //     .WithMany(p => p.Profilings)
            //     .HasForeignKey(p => p.Education_id);

            builder.Entity<Education>()
                .HasMany(e => e.Profilings)
                .WithOne(e => e.Education)
                .HasForeignKey(e => e.Education_id)
                .IsRequired();
            
            builder.Entity<Education>()
                .HasOne(e => e.University)
                .WithMany(e => e.Educations)
                .HasForeignKey(e => e.University_Id)
                .IsRequired();
        }
    }
}