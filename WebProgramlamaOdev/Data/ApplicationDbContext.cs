using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebProgramlamaOdev.Models;

namespace WebProgramlamaOdev.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet Tanımlamaları
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<ServicesByTrainer> ServicesByTrainers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<TrainerAvailability> TrainerAvailabilities { get; set; }
        public DbSet<AIDailyPlanRecommendation> AIDailyPlanRecommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ApplicationUser ile Member ilişkisi (1:1)
            modelBuilder.Entity<Member>()
                .HasOne(m => m.User)
                .WithOne(u => u.Member)
                .HasForeignKey<Member>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser ile Trainer ilişkisi (1:1)
            modelBuilder.Entity<Trainer>()
                .HasOne(t => t.User)
                .WithOne(u => u.Trainer)
                .HasForeignKey<Trainer>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApplicationUser ile Gym ilişkisi (1:1)
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Gym)
                .WithOne(g => g.User)
                .HasForeignKey<Gym>(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Gym ile Trainer ilişkisi (1:N)
            modelBuilder.Entity<Trainer>()
                .HasOne(t => t.Gym)
                .WithMany(g => g.TrainerList)
                .HasForeignKey(t => t.GymId)
                .OnDelete(DeleteBehavior.Restrict);

            // Gym ile ServiceType ilişkisi (1:N)
            modelBuilder.Entity<ServiceType>()
                .HasOne(s => s.Gym)
                .WithMany(g => g.ServiceList)
                .HasForeignKey(s => s.GymId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServicesByTrainer - Trainer ilişkisi (Many-to-Many junction table)
            modelBuilder.Entity<ServicesByTrainer>()
                .HasOne(st => st.Trainer)
                .WithMany()
                .HasForeignKey(st => st.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServicesByTrainer - ServiceType ilişkisi
            modelBuilder.Entity<ServicesByTrainer>()
                .HasOne(st => st.serviceType)
                .WithMany(s => s.ServicesByTrainers)
                .HasForeignKey(st => st.ServiceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - Member ilişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Member)
                .WithMany(m => m.Appointments)
                .HasForeignKey(a => a.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - Trainer ilişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Trainer)
                .WithMany(t => t.Appointments)
                .HasForeignKey(a => a.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - ServiceType ilişkisi
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.ServiceType)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // TrainerAvailability - Trainer ilişkisi
            modelBuilder.Entity<TrainerAvailability>()
                .HasOne(ta => ta.Trainer)
                .WithMany(t => t.Availabilities)
                .HasForeignKey(ta => ta.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

            // AIDailyPlanRecommendation - Member ilişkisi
            modelBuilder.Entity<AIDailyPlanRecommendation>()
                .HasOne(ai => ai.Member)
                .WithMany(m => m.AIRecommendations)
                .HasForeignKey(ai => ai.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique Indexes
            modelBuilder.Entity<Gym>()
                .HasIndex(g => g.UserId)
                .IsUnique();

            modelBuilder.Entity<Member>()
                .HasIndex(m => m.UserId)
                .IsUnique();

            modelBuilder.Entity<Trainer>()
                .HasIndex(t => t.UserId)
                .IsUnique();

            // Performance Indexes
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.TrainerId, a.AppointmentDate, a.StartTime })
                .HasDatabaseName("IX_Appointment_Trainer_Date_Time");

            modelBuilder.Entity<TrainerAvailability>()
                .HasIndex(ta => new { ta.TrainerId, ta.EventDate })
                .HasDatabaseName("IX_TrainerAvailability_Trainer_Date");

            modelBuilder.Entity<AIDailyPlanRecommendation>()
                .HasIndex(ai => ai.MemberId)
                .HasDatabaseName("IX_AIRecommendation_Member");

            modelBuilder.Entity<ServiceType>()
                .HasIndex(s => s.IsActive)
                .HasDatabaseName("IX_ServiceType_IsActive");

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.Status)
                .HasDatabaseName("IX_Appointment_Status");

            // Precision Configuration
            modelBuilder.Entity<ServiceType>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Appointment>()
                .Property(a => a.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Member>()
                .Property(m => m.Weight)
                .HasPrecision(5, 2);

            // Default Values
            modelBuilder.Entity<Appointment>()
                .Property(a => a.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<AIDailyPlanRecommendation>()
                .Property(ai => ai.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            // ========================================
            // ✅ SEED DATA - BURAYA EKLENDİ
            // ========================================
            SeedData(modelBuilder);
        }

        // ========================================
        // SEED DATA METODU
        // ========================================
        private void SeedData(ModelBuilder modelBuilder)
        {
            // User ID'leri
            var adminUserId = "admin-001";
            var gym1UserId = "gym-001";
            var gym2UserId = "gym-002";
            var trainer1UserId = "trainer-001";
            var trainer2UserId = "trainer-002";
            var trainer3UserId = "trainer-003";
            var member1UserId = "member-001";
            var member2UserId = "member-002";
            var member3UserId = "member-003";

            // ========================================
            // 1. APPLICATION USERS
            // ========================================
            modelBuilder.Entity<ApplicationUser>().HasData(
                // Admin
                new ApplicationUser
                {
                    Id = adminUserId,
                    UserName = "admin@gym.com",
                    NormalizedUserName = "ADMIN@GYM.COM",
                    Email = "admin@gym.com",
                    NormalizedEmail = "ADMIN@GYM.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "05551234567",
                    FirstName = "System",
                    LastName = "Admin",
                    UserType = "Admin",
                    CreatedDate = new DateTime(2024, 1, 1),
                    PasswordHash = "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                // Gym 1 User
                new ApplicationUser
                {
                    Id = gym1UserId,
                    UserName = "sakarya@gym.com",
                    NormalizedUserName = "SAKARYA@GYM.COM",
                    Email = "sakarya@gym.com",
                    NormalizedEmail = "SAKARYA@GYM.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "05551112233",
                    FirstName = "Gym",
                    LastName = "Admin",
                    UserType = "Gym",
                    CreatedDate = new DateTime(2024, 1, 1),
                    PasswordHash = "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                // Gym 2 User
                new ApplicationUser
                {
                    Id = gym2UserId,
                    UserName = "adapazari@gym.com",
                    NormalizedUserName = "ADAPAZARI@GYM.COM",
                    Email = "adapazari@gym.com",
                    NormalizedEmail = "ADAPAZARI@GYM.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "05559998877",
                    FirstName = "Gym",
                    LastName = "Admin",
                    UserType = "Gym",
                    CreatedDate = new DateTime(2024, 1, 1),
                    PasswordHash = "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                // Trainer 1 User
                new ApplicationUser
                {
                    Id = trainer1UserId,
                    UserName = "ahmet.yilmaz@gym.com",
                    NormalizedUserName = "AHMET.YILMAZ@GYM.COM",
                    Email = "ahmet.yilmaz@gym.com",
                    NormalizedEmail = "AHMET.YILMAZ@GYM.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "05551234501",
                    FirstName = "Ahmet",
                    LastName = "Yılmaz",
                    UserType = "Trainer",
                    CreatedDate = new DateTime(2024, 1, 1),
                    PasswordHash = "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                // Trainer 2 User
                new ApplicationUser
                {
                    Id = trainer2UserId,
                    UserName = "mehmet.demir@gym.com",
                    NormalizedUserName = "MEHMET.DEMIR@GYM.COM",
                    Email = "mehmet.demir@gym.com",
                    NormalizedEmail = "MEHMET.DEMIR@GYM.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "05551234502",
                    FirstName = "Mehmet",
                    LastName = "Demir",
                    UserType = "Trainer",
                    CreatedDate = new DateTime(2024, 1, 1),
                    PasswordHash = "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                // Trainer 3 User
                new ApplicationUser
                {
                    Id = trainer3UserId,
                    UserName = "ayse.kara@gym.com",
                    NormalizedUserName = "AYSE.KARA@GYM.COM",
                    Email = "ayse.kara@gym.com",
                    NormalizedEmail = "AYSE.KARA@GYM.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "05551234503",
                    FirstName = "Ayşe",
                    LastName = "Kara",
                    UserType = "Trainer",
                    CreatedDate = new DateTime(2024, 1, 1),
                    PasswordHash = "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                // Member 1 User
                new ApplicationUser
                {
                    Id = member1UserId,
                    UserName = "ali.veli@test.com",
                    NormalizedUserName = "ALI.VELI@TEST.COM",
                    Email = "ali.veli@test.com",
                    NormalizedEmail = "ALI.VELI@TEST.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "05559876501",
                    FirstName = "Ali",
                    LastName = "Veli",
                    UserType = "Member",
                    CreatedDate = new DateTime(2024, 1, 1),
                    PasswordHash = "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                // Member 2 User
                new ApplicationUser
                {
                    Id = member2UserId,
                    UserName = "zeynep.can@test.com",
                    NormalizedUserName = "ZEYNEP.CAN@TEST.COM",
                    Email = "zeynep.can@test.com",
                    NormalizedEmail = "ZEYNEP.CAN@TEST.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "05559876502",
                    FirstName = "Zeynep",
                    LastName = "Can",
                    UserType = "Member",
                    CreatedDate = new DateTime(2024, 1, 1),
                    PasswordHash = "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                // Member 3 User
                new ApplicationUser
                {
                    Id = member3UserId,
                    UserName = "can.ozturk@test.com",
                    NormalizedUserName = "CAN.OZTURK@TEST.COM",
                    Email = "can.ozturk@test.com",
                    NormalizedEmail = "CAN.OZTURK@TEST.COM",
                    EmailConfirmed = true,
                    PhoneNumber = "05559876503",
                    FirstName = "Can",
                    LastName = "Öztürk",
                    UserType = "Member",
                    CreatedDate = new DateTime(2024, 1, 1),
                    PasswordHash = "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );

            // ========================================
            // 2. GYMS
            // ========================================
            modelBuilder.Entity<Gym>().HasData(
                new Gym
                {
                    Id = 1,
                    UserId = gym1UserId,
                    Name = "Sakarya Fitness Center",
                    Address = "Sakarya Üniversitesi Esentepe Kampüsü, Serdivan/Sakarya",
                    OpeningTime = new TimeSpan(6, 0, 0),
                    ClosingTime = new TimeSpan(23, 0, 0),
                    IsActive = true
                },
                new Gym
                {
                    Id = 2,
                    UserId = gym2UserId,
                    Name = "Adapazarı Spor Salonu",
                    Address = "Merkez Mah. Atatürk Cad. No:123, Adapazarı/Sakarya",
                    OpeningTime = new TimeSpan(7, 0, 0),
                    ClosingTime = new TimeSpan(22, 0, 0),
                    IsActive = true
                }
            );

            // ========================================
            // 3. MEMBERS
            // ========================================
            modelBuilder.Entity<Member>().HasData(
                new Member
                {
                    Id = 1,
                    UserId = member1UserId,
                    DateOfBirth = new DateTime(1995, 5, 15),
                    Gender = "Erkek",
                    Height = 180,
                    Weight = 75.5m
                },
                new Member
                {
                    Id = 2,
                    UserId = member2UserId,
                    DateOfBirth = new DateTime(1998, 8, 22),
                    Gender = "Kadın",
                    Height = 165,
                    Weight = 58.0m
                },
                new Member
                {
                    Id = 3,
                    UserId = member3UserId,
                    DateOfBirth = new DateTime(1992, 3, 10),
                    Gender = "Erkek",
                    Height = 175,
                    Weight = 80.0m
                }
            );

            // ========================================
            // 4. TRAINERS
            // ========================================
            modelBuilder.Entity<Trainer>().HasData(
                new Trainer
                {
                    Id = 1,
                    UserId = trainer1UserId,
                    GymId = 1,
                    Specialization = "Kişisel Antrenman, Güç Antrenmanı",
                    IsActive = true
                },
                new Trainer
                {
                    Id = 2,
                    UserId = trainer2UserId,
                    GymId = 1,
                    Specialization = "Pilates, Yoga, Esneklik",
                    IsActive = true
                },
                new Trainer
                {
                    Id = 3,
                    UserId = trainer3UserId,
                    GymId = 2,
                    Specialization = "Crossfit, Fonksiyonel Antrenman",
                    IsActive = true
                }
            );

            // ========================================
            // 5. SERVICE TYPES (GÜNCELLEME)
            // ========================================
            modelBuilder.Entity<ServiceType>().HasData(
                // Gym 1 Services
                new ServiceType
                {
                    Id = 1,
                    GymId = 1,
                    Name = "Kişisel Antrenman",
                    Description = "Birebir kişisel antrenman seansı. Antrenörünüz sizin için özel program hazırlar.",
                    Duration = 60,
                    Price = 200.00m,
                    IsActive = true
                },
                new ServiceType
                {
                    Id = 2,
                    GymId = 1,
                    Name = "Grup Dersi - Pilates",
                    Description = "Grup halinde pilates antrenmanı. Maksimum 10 kişilik gruplar.",
                    Duration = 45,
                    Price = 100.00m,
                    IsActive = true
                },
                new ServiceType
                {
                    Id = 3,
                    GymId = 1,
                    Name = "Beslenme Danışmanlığı",
                    Description = "Diyet ve beslenme programı oluşturma. Kişiye özel beslenme planı.",
                    Duration = 30,
                    Price = 150.00m,
                    IsActive = true
                },
                new ServiceType
                {
                    Id = 4,
                    GymId = 1,
                    Name = "Grup Dersi - Spinning",
                    Description = "Yüksek tempolu kardiyo antrenmanı. Maksimum 15 kişilik gruplar.",
                    Duration = 45,
                    Price = 90.00m,
                    IsActive = true
                },
                new ServiceType
                {
                    Id = 5,
                    GymId = 1,
                    Name = "Yoga",
                    Description = "Nefes egzersizleri ve esneme hareketleri ile zihin ve beden dinginliği.",
                    Duration = 60,
                    Price = 120.00m,
                    IsActive = true
                },
                // Gym 2 Services
                new ServiceType
                {
                    Id = 6,
                    GymId = 2,
                    Name = "Crossfit",
                    Description = "Yüksek yoğunluklu fonksiyonel antrenman",
                    Duration = 60,
                    Price = 180.00m,
                    IsActive = true
                },
                new ServiceType
                {
                    Id = 7,
                    GymId = 2,
                    Name = "Kardio Antrenmanı",
                    Description = "Dayanıklılık ve kondisyon geliştirme",
                    Duration = 45,
                    Price = 130.00m,
                    IsActive = true
                }
            );

            // ========================================
            // 6. SERVICES BY TRAINER
            // ========================================
            modelBuilder.Entity<ServicesByTrainer>().HasData(
                // Trainer 1 - Kişisel Antrenman + Beslenme
                new ServicesByTrainer { Id=-1,TrainerId = 1, ServiceTypeId = 1 },
                new ServicesByTrainer { Id = -2,TrainerId = 1, ServiceTypeId = 3 },
                // Trainer 2 - Pilates + Yoga
                new ServicesByTrainer { Id = -3, TrainerId = 2, ServiceTypeId = 2 },
                new ServicesByTrainer { Id = -4, TrainerId = 2, ServiceTypeId = 5 },
                // Trainer 3 - Crossfit + Kardio
                new ServicesByTrainer { Id = -5,TrainerId = 3, ServiceTypeId = 6 },
                new ServicesByTrainer { Id = -6, TrainerId = 3, ServiceTypeId = 7 }
            );

            // ========================================
            // 7. TRAINER AVAILABILITIES
            // ========================================
            var today = DateTime.Today;
            modelBuilder.Entity<TrainerAvailability>().HasData(
                // Trainer 1 Availabilities
                new TrainerAvailability
                {
                    Id = 1,
                    TrainerId = 1,
                    EventDate = today.AddDays(1),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(12, 0, 0),
                    IsActive = true
                },
                new TrainerAvailability
                {
                    Id = 2,
                    TrainerId = 1,
                    EventDate = today.AddDays(1),
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0),
                    IsActive = true
                },
                new TrainerAvailability
                {
                    Id = 3,
                    TrainerId = 1,
                    EventDate = today.AddDays(2),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    IsActive = true
                },
                // Trainer 2 Availabilities
                new TrainerAvailability
                {
                    Id = 4,
                    TrainerId = 2,
                    EventDate = today.AddDays(1),
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(16, 0, 0),
                    IsActive = true
                },
                new TrainerAvailability
                {
                    Id = 5,
                    TrainerId = 2,
                    EventDate = today.AddDays(3),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(13, 0, 0),
                    IsActive = true
                },
                // Trainer 3 Availabilities
                new TrainerAvailability
                {
                    Id = 6,
                    TrainerId = 3,
                    EventDate = today.AddDays(1),
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(20, 0, 0),
                    IsActive = true
                }
            );

            // ========================================
            // 8. APPOINTMENTS (Opsiyonel)
            // ========================================
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    Id = 1,
                    MemberId = 1,
                    TrainerId = 1,
                    ServiceTypeId = 1,
                    AppointmentDate = today.AddDays(2),
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(11, 0, 0),
                    Status = "Confirmed",
                    TotalPrice = 200.00m,
                    IsConfirmed = true,
                    CreatedDate = today.AddDays(-1)
                },
                new Appointment
                {
                    Id = 2,
                    MemberId = 2,
                    TrainerId = 2,
                    ServiceTypeId = 2,
                    AppointmentDate = today.AddDays(1),
                    StartTime = new TimeSpan(11, 0, 0),
                    EndTime = new TimeSpan(11, 45, 0),
                    Status = "Pending",
                    TotalPrice = 100.00m,
                    IsConfirmed = false,
                    CreatedDate = today
                }
            );
        }
    }
}