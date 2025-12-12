using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebProgramlamaOdev.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gyms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OpeningTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gyms_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Member_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GymId = table.Column<int>(type: "int", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trainer_Gyms_GymId",
                        column: x => x.GymId,
                        principalTable: "Gyms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AIDailyPlanRecommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InputData = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    AIResponse = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    GeneratedImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIDailyPlanRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIDailyPlanRecommendations_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GymId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceType_Gyms_GymId",
                        column: x => x.GymId,
                        principalTable: "Gyms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceType_Trainer_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainerAvailability",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerAvailability_Trainer_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_ServiceType_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointment_Trainer_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicesByTrainer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesByTrainer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicesByTrainer_ServiceType_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicesByTrainer_Trainer_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedDate", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "UserType" },
                values: new object[,]
                {
                    { "admin-001", 0, "61c035e9-c74e-4c0e-a213-62a99e5dc165", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gym.com", true, "System", "Admin", false, null, "ADMIN@GYM.COM", "ADMIN@GYM.COM", "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==", "05551234567", false, "3836e5fa-b84e-462e-834e-789f93d4fde2", false, "admin@gym.com", "Admin" },
                    { "gym-001", 0, "0e711bfe-ff28-4fe4-bd24-bc2bdc756da6", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sakarya@gym.com", true, "Gym", "Admin", false, null, "SAKARYA@GYM.COM", "SAKARYA@GYM.COM", "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==", "05551112233", false, "b03b5d46-77c0-44a6-abfd-513965b6f5ad", false, "sakarya@gym.com", "Gym" },
                    { "gym-002", 0, "57634f8e-15f8-4155-b388-d62c65595506", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "adapazari@gym.com", true, "Gym", "Admin", false, null, "ADAPAZARI@GYM.COM", "ADAPAZARI@GYM.COM", "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==", "05559998877", false, "a8d07e10-ff83-4197-b848-3ad0b1553109", false, "adapazari@gym.com", "Gym" },
                    { "member-001", 0, "fbb81d9f-13db-4853-aee4-f7b37bdd3c05", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ali.veli@test.com", true, "Ali", "Veli", false, null, "ALI.VELI@TEST.COM", "ALI.VELI@TEST.COM", "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==", "05559876501", false, "e1e98c42-763a-42bd-915f-179999fe12f9", false, "ali.veli@test.com", "Member" },
                    { "member-002", 0, "cefe08ab-3f2c-4096-9076-97a353410581", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "zeynep.can@test.com", true, "Zeynep", "Can", false, null, "ZEYNEP.CAN@TEST.COM", "ZEYNEP.CAN@TEST.COM", "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==", "05559876502", false, "82dfbcaa-9bae-47b7-8b81-08c302091598", false, "zeynep.can@test.com", "Member" },
                    { "member-003", 0, "f695dd09-d5ec-4397-949f-c73d5d602991", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "can.ozturk@test.com", true, "Can", "Öztürk", false, null, "CAN.OZTURK@TEST.COM", "CAN.OZTURK@TEST.COM", "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==", "05559876503", false, "c9c4044a-40af-4fef-9145-33ada55e8cc7", false, "can.ozturk@test.com", "Member" },
                    { "trainer-001", 0, "fe116134-cff5-46ee-a224-f666021bdcdd", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahmet.yilmaz@gym.com", true, "Ahmet", "Yılmaz", false, null, "AHMET.YILMAZ@GYM.COM", "AHMET.YILMAZ@GYM.COM", "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==", "05551234501", false, "96b0a302-1388-4568-9dc1-3743f9ad746d", false, "ahmet.yilmaz@gym.com", "Trainer" },
                    { "trainer-002", 0, "8654c29c-198f-4b8f-8d42-bd3cc9fbd2fd", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mehmet.demir@gym.com", true, "Mehmet", "Demir", false, null, "MEHMET.DEMIR@GYM.COM", "MEHMET.DEMIR@GYM.COM", "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==", "05551234502", false, "f7471a13-43b2-480f-ae7f-60aa16751c4c", false, "mehmet.demir@gym.com", "Trainer" },
                    { "trainer-003", 0, "4ee2a550-55cf-4091-bbca-8fcf4b0cb142", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ayse.kara@gym.com", true, "Ayşe", "Kara", false, null, "AYSE.KARA@GYM.COM", "AYSE.KARA@GYM.COM", "AQAAAAIAAYagAAAAELqP9QqP0yH8XxJHLKJv3xK+5xL6Y3sR8vN9wT4mP2xQ7zR1sK0vB5nM3cL9dT6eA==", "05551234503", false, "6a455e26-6b06-4bd4-b14f-b1b25c92f622", false, "ayse.kara@gym.com", "Trainer" }
                });

            migrationBuilder.InsertData(
                table: "Gyms",
                columns: new[] { "Id", "Address", "ClosingTime", "IsActive", "Name", "OpeningTime", "UserId" },
                values: new object[,]
                {
                    { 1, "Sakarya Üniversitesi Esentepe Kampüsü, Serdivan/Sakarya", new TimeSpan(0, 23, 0, 0, 0), true, "Sakarya Fitness Center", new TimeSpan(0, 6, 0, 0, 0), "gym-001" },
                    { 2, "Merkez Mah. Atatürk Cad. No:123, Adapazarı/Sakarya", new TimeSpan(0, 22, 0, 0, 0), true, "Adapazarı Spor Salonu", new TimeSpan(0, 7, 0, 0, 0), "gym-002" }
                });

            migrationBuilder.InsertData(
                table: "Member",
                columns: new[] { "Id", "DateOfBirth", "Gender", "Height", "UserId", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(1995, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Erkek", 180, "member-001", 75.5m },
                    { 2, new DateTime(1998, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kadın", 165, "member-002", 58.0m },
                    { 3, new DateTime(1992, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Erkek", 175, "member-003", 80.0m }
                });

            migrationBuilder.InsertData(
                table: "ServiceType",
                columns: new[] { "Id", "Description", "Duration", "GymId", "IsActive", "Name", "Price", "TrainerId" },
                values: new object[,]
                {
                    { 1, "Birebir kişisel antrenman seansı. Antrenörünüz sizin için özel program hazırlar.", 60, 1, true, "Kişisel Antrenman", 200.00m, null },
                    { 2, "Grup halinde pilates antrenmanı. Maksimum 10 kişilik gruplar.", 45, 1, true, "Grup Dersi - Pilates", 100.00m, null },
                    { 3, "Diyet ve beslenme programı oluşturma. Kişiye özel beslenme planı.", 30, 1, true, "Beslenme Danışmanlığı", 150.00m, null },
                    { 4, "Yüksek tempolu kardiyo antrenmanı. Maksimum 15 kişilik gruplar.", 45, 1, true, "Grup Dersi - Spinning", 90.00m, null },
                    { 5, "Nefes egzersizleri ve esneme hareketleri ile zihin ve beden dinginliği.", 60, 1, true, "Yoga", 120.00m, null },
                    { 6, "Yüksek yoğunluklu fonksiyonel antrenman", 60, 2, true, "Crossfit", 180.00m, null },
                    { 7, "Dayanıklılık ve kondisyon geliştirme", 45, 2, true, "Kardio Antrenmanı", 130.00m, null }
                });

            migrationBuilder.InsertData(
                table: "Trainer",
                columns: new[] { "Id", "ApplicationUser", "GymId", "IsActive", "Specialization", "UserId" },
                values: new object[,]
                {
                    { 1, null, 1, true, "Kişisel Antrenman, Güç Antrenmanı", "trainer-001" },
                    { 2, null, 1, true, "Pilates, Yoga, Esneklik", "trainer-002" },
                    { 3, null, 2, true, "Crossfit, Fonksiyonel Antrenman", "trainer-003" }
                });

            migrationBuilder.InsertData(
                table: "Appointment",
                columns: new[] { "Id", "AppointmentDate", "CreatedDate", "EndTime", "IsConfirmed", "MemberId", "ServiceTypeId", "StartTime", "Status", "TotalPrice", "TrainerId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 12, 11, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 11, 0, 0, 0), true, 1, 1, new TimeSpan(0, 10, 0, 0, 0), "Confirmed", 200.00m, 1 },
                    { 2, new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new TimeSpan(0, 11, 45, 0, 0), false, 2, 2, new TimeSpan(0, 11, 0, 0, 0), "Pending", 100.00m, 2 }
                });

            migrationBuilder.InsertData(
                table: "ServicesByTrainer",
                columns: new[] { "Id", "ServiceTypeId", "TrainerId" },
                values: new object[,]
                {
                    { -6, 7, 3 },
                    { -5, 6, 3 },
                    { -4, 5, 2 },
                    { -3, 2, 2 },
                    { -2, 3, 1 },
                    { -1, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "TrainerAvailability",
                columns: new[] { "Id", "EndTime", "EventDate", "IsActive", "StartTime", "TrainerId" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 12, 0, 0, 0), new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), true, new TimeSpan(0, 9, 0, 0, 0), 1 },
                    { 2, new TimeSpan(0, 18, 0, 0, 0), new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), true, new TimeSpan(0, 14, 0, 0, 0), 1 },
                    { 3, new TimeSpan(0, 17, 0, 0, 0), new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Local), true, new TimeSpan(0, 9, 0, 0, 0), 1 },
                    { 4, new TimeSpan(0, 16, 0, 0, 0), new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), true, new TimeSpan(0, 10, 0, 0, 0), 2 },
                    { 5, new TimeSpan(0, 13, 0, 0, 0), new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Local), true, new TimeSpan(0, 9, 0, 0, 0), 2 },
                    { 6, new TimeSpan(0, 20, 0, 0, 0), new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), true, new TimeSpan(0, 8, 0, 0, 0), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIRecommendation_Member",
                table: "AIDailyPlanRecommendations",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_MemberId",
                table: "Appointment",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ServiceTypeId",
                table: "Appointment",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_Status",
                table: "Appointment",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_Trainer_Date_Time",
                table: "Appointment",
                columns: new[] { "TrainerId", "AppointmentDate", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Gyms_UserId",
                table: "Gyms",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_UserId",
                table: "Member",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServicesByTrainer_ServiceTypeId",
                table: "ServicesByTrainer",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesByTrainer_TrainerId",
                table: "ServicesByTrainer",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceType_GymId",
                table: "ServiceType",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceType_IsActive",
                table: "ServiceType",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceType_TrainerId",
                table: "ServiceType",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainer_GymId",
                table: "Trainer",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainer_UserId",
                table: "Trainer",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainerAvailability_Trainer_Date",
                table: "TrainerAvailability",
                columns: new[] { "TrainerId", "EventDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIDailyPlanRecommendations");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ServicesByTrainer");

            migrationBuilder.DropTable(
                name: "TrainerAvailability");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ServiceType");

            migrationBuilder.DropTable(
                name: "Trainer");

            migrationBuilder.DropTable(
                name: "Gyms");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
