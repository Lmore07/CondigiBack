using CondigiBack.Libs.Enums;
using CondigiBack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql;

namespace CondigiBack.Contexts
{
    public class AppDBContext: DbContext
    {
        
        public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {
        }
       
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Canton> Cantons { get; set; }
        public DbSet<Parish> Parishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PROVINCE

            modelBuilder.Entity<Province>()
                .HasKey(p => p.Id);

            // CANTON

            modelBuilder.Entity<Canton>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Canton>()
                .HasOne(c => c.Province)
                .WithMany(p => p.Cantons)
                .HasForeignKey(c => c.ProvinceId);

            // PARISH

            modelBuilder.Entity<Parish>()
                .HasKey(p => p.IdParish);

            modelBuilder.Entity<Parish>()
                .HasOne(p => p.Canton)
                .WithMany(c => c.Parishes)
                .HasForeignKey(p => p.CantonId);

            // PERSON

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Parish)
                .WithMany(c => c.Persons)
                .HasForeignKey(p => p.ParishId);

            modelBuilder.Entity<Person>()
                .HasKey(p => p.Id);

            // USER
            modelBuilder.Entity<User>()
                .HasOne(u => u.Person)
                .WithOne(p => p.User)
                .HasForeignKey<User>(u => u.PersonId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // CONTRACT

            modelBuilder.Entity<Contract>()
               .HasKey(c => c.Id);

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.User)
                .WithMany(u => u.Contracts)
                .HasForeignKey(c => c.UserId)
                .HasConstraintName("FK_Contract_User");

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Counterparty)
                .WithMany()
                .HasForeignKey(c => c.CounterpartyId)
                .HasConstraintName("FK_Contracts_Users_Counterparty");

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.ContractType)
                .WithMany(ct => ct.Contracts)
                .HasForeignKey(c => c.ContractTypeId);

            // CONTRACT TYPE

            modelBuilder.Entity<ContractType>()
                .HasKey(ct => ct.Id);
        }
    }
}
