using CondigiBack.Models;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Company> Companies { get; set; }
        public DbSet<ContractParticipant> ContractParticipants { get; set; }
        public DbSet<UserCompanies> UserCompanies { get; set; }

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
                .HasOne(c => c.ContractType)
                .WithMany(ct => ct.Contracts)
                .HasForeignKey(c => c.ContractTypeId);

            // CONTRACT TYPE

            modelBuilder.Entity<ContractType>()
                .HasKey(ct => ct.Id);

            //COMPANY

            modelBuilder.Entity<Company>()
                .HasKey(c => c.Id);

            //UserCompanies

            modelBuilder.Entity<UserCompanies>()
                .HasKey(uc => uc.Id);

            modelBuilder.Entity<UserCompanies>()
                .HasOne(uc => uc.Company)
                .WithMany(c => c.UserCompanies)
                .HasForeignKey(uc => uc.CompanyId);

            modelBuilder.Entity<UserCompanies>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCompanies)
                .HasForeignKey(u => u.UserId);

            //Contract Participants

            modelBuilder.Entity<ContractParticipant>()
                .HasKey(c => c.Id);

            //relacion con contratos
            modelBuilder.Entity<ContractParticipant>()
                .HasOne(cp => cp.Contract)
                .WithMany(c => c.ContractParticipants)
                .HasForeignKey(cp => cp.ContracId);

            //relacion con usuarios
            modelBuilder.Entity<ContractParticipant>()
                .HasOne(cp => cp.User)
                .WithMany(u => u.ContractParticipants)
                .HasForeignKey(cp => cp.UserId);

            //relacion con compañias (opcional)
            modelBuilder.Entity<ContractParticipant>()
                .HasOne(cp => cp.Company)
                .WithMany(c => c.ContractParticipants)
                .HasForeignKey(cp => cp.CompanyId);
        }
    }
}
