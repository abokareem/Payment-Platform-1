using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Models;

namespace PaymentPlatform.Framework.Services.RandomDataGenerator.Context
{
    /// <summary>
    /// Основной контекст приложения.
    /// </summary>
    public class MainContext : DbContext
    {
        /// <summary>
        /// Аккаунты.
        /// </summary>
        public DbSet<AccountContextModel> Accounts { get; set; }

        /// <summary>
        /// Резервирования баланса.
        /// </summary>
        public DbSet<BalanceReservedContextModel> BalanceReserveds { get; set; }

        /// <summary>
        /// Товары.
        /// </summary>
        public DbSet<ProductContextModel> Products { get; set; }

        /// <summary>
        /// Резервирования товаров.
        /// </summary>
        public DbSet<ProductReservedContextModel> ProductReserveds { get; set; }

        /// <summary>
        /// Профили.
        /// </summary>
        public DbSet<ProfileContextModel> Profiles { get; set; }

        /// <summary>
        /// Транзакции.
        /// </summary>
        public DbSet<TransactionContextModel> Transactions { get; set; }

        /// <summary>
        /// Таблица Serilog.
        /// </summary>
        public DbSet<SerilogContextModel> Serilogs { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public MainContext() => Database.EnsureCreated();

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="options">Настройки MainContext.</param>
        public MainContext(DbContextOptions<MainContext> options)
            : base(options) => Database.EnsureCreated();

        /// <summary>
        /// Реализация FluentAPI.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Account table

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Email)
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Password)
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Login)
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Role)
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.IsActive)
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .HasOne(p => p.Profile)
                .WithOne(a => a.Account)
                .HasForeignKey<ProfileContextModel>(p => p.Id);

            #endregion Account table

            #region BalanceReserve table

            modelBuilder.Entity<BalanceReservedContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();

            #endregion BalanceReserve table

            #region ProductReserve table

            modelBuilder.Entity<ProductReservedContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();

            #endregion ProductReserve table

            #region Profile table

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.FirstName)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.LastName)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.SecondName);

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.Passport);

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.IsSeller)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.OrgName);

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.OrgNumber);

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.BankBook)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.Balance)
                .IsRequired();

            #endregion Profile table

            #region Product table

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.ProfileId)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Description)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.MeasureUnit)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Category)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Amount)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Price)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.QrCode)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.IsActive)
                .IsRequired();

            #endregion Product table

            #region Transaction table

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.ProfileId)
                .IsRequired();

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.ProductId)
                .IsRequired();

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.TransactionTime)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.Status)
                .IsRequired();

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.TotalCost)
                .IsRequired();

            modelBuilder.Entity<TransactionContextModel>()
                .HasOne(t => t.Profile)
                .WithMany(s => s.Transactions)
                .HasForeignKey(p => p.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionContextModel>()
                .HasOne(t => t.Product)
                .WithMany(s => s.Transactions)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionContextModel>()
                .HasOne(t => t.BalanceReserved)
                .WithOne(s => s.Transaction)
                .HasForeignKey<BalanceReservedContextModel>(p => p.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionContextModel>()
                .HasOne(pr => pr.ProductReserved)
                .WithOne(t => t.Transaction)
                .HasForeignKey<ProductReservedContextModel>(pr => pr.Id)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion Transaction table

            #region Serilog table

            modelBuilder.Entity<SerilogContextModel>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<SerilogContextModel>()
                .Property(p => p.Message);

            modelBuilder.Entity<SerilogContextModel>()
                .Property(p => p.MessageTemplate);

            modelBuilder.Entity<SerilogContextModel>()
                .Property(p => p.Level)
                .HasMaxLength(128);

            modelBuilder.Entity<SerilogContextModel>()
                .Property(p => p.TimeStamp)
                .HasColumnType("datetime")
                .IsRequired();

            modelBuilder.Entity<SerilogContextModel>()
                .Property(p => p.Exception);

            modelBuilder.Entity<SerilogContextModel>()
                .Property(p => p.Properties);

            #endregion Serilog table
        }
    }
}