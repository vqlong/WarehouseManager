using Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Model;
using System;
using System.Linq;
using System.Security.AccessControl;

namespace EntityDataAccess
{
    public class WarehouseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Suplier> Supliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Input> Inputs { get; set; }
        public DbSet<InputInfo> InputInfos { get; set; } 
        public DbSet<Output> Outputs { get; set; }
        public DbSet<OutputInfo> OutputInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;Initial Catalog=WarehouseManager;Integrated Security=True;Encrypt=False");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(build =>
            {
                build.Property(a => a.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                build.Property(a => a.DisplayName)
                        .HasDefaultValueSql("N'CafeNo1'")
                        .HasMaxLength(50);

                build.Property(u => u.Password)
                    .HasDefaultValueSql("N'1661011648932664715765126721032392207918416074316325531160126153142134247247162122227'") //123
                    .HasConversion(password => password.ToSHA256(), password => "******");

                build.Property(a => a.Role)
                    .HasDefaultValue(Role.Staff)
                    .HasConversion<int>();

                build.Property(i => i.DateInserted)
                    .HasDefaultValueSql("GETDATE()");

                build.HasData(new User { Id = 1, Username = "admin", Role = Role.Admin, DisplayName = "Chủ kho", Password = "123" });
            });

            modelBuilder.Entity<Input>(build =>
            {
                build.Property(i => i.Date)
                    .HasDefaultValueSql("GETDATE()");
                build.Property(i => i.SerialNumber)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    .HasValueGenerator(typeof(SequentialGuidValueGenerator)); 
            });

            modelBuilder.Entity<InputInfo>(build =>
            {
                build.Property(ii => ii.Status)
                    .HasDefaultValueSql("'OK'");
            });

            modelBuilder.Entity<Output>(build =>
            {
                build.Property(i => i.Date)
                    .HasDefaultValueSql("GETDATE()");
                build.Property(i => i.SerialNumber)
                    .HasDefaultValueSql("NEWSEQUENTIALID()")
                    .HasValueGenerator(typeof(SequentialGuidValueGenerator));
            });

            modelBuilder.Entity<OutputInfo>(build =>
            {
                build.Property(oi => oi.Status)
                    .HasDefaultValueSql("'OK'");
            });

            modelBuilder.Entity<Suplier>(build =>
            {
                build.Property(i => i.ContractDate)
                    .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Customer>(build =>
            {
                build.Property(i => i.ContractDate)
                    .HasDefaultValueSql("GETDATE()");
            });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            //Tìm nhứng entity được insert
            var insertedEntities = this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).Select(e => e.Entity);
            //Nếu nó kế thừa Model thì gán giá trị
            foreach (var entity in insertedEntities)
            {
                if (entity is Model.Model model) model.DateInserted = DateTime.Now;
            }

            var updatedEntities = this.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).Select(e => e.Entity);
            foreach (var entity in updatedEntities)
            {
                if (entity is Model.Model model) model.DateUpdated = DateTime.Now;
            }

            var deletedEntities = this.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).Select(e => e.Entity);
            //Không xoá thực sự mà chỉ chuyển IsDeleted = true
            foreach (var entity in deletedEntities)
            {
                if (entity is Model.Model model)
                {
                    model.IsDeleted = true;

                    this.Entry(model).State = EntityState.Unchanged;
                    this.Entry(model).Property(m => m.IsDeleted).IsModified = true;
                }
            }

            return base.SaveChanges();
        }
    }
}
