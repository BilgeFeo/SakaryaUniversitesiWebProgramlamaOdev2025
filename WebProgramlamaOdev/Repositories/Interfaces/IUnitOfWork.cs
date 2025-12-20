using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Repositories.Interfaces
{
    /// <summary>
    /// Unit of Work Pattern - Transaction management ve tüm repository'leri yönetir
    /// </summary>
    public interface IUnitOfWork : IDisposable

        
    {


        ApplicationDbContext Context { get; }
        // Repository Properties
        IMemberRepository Members { get; }
        ITrainerRepository Trainers { get; }
        IGymRepository Gyms { get; }
        IServiceTypeRepository ServiceTypes { get; }
        IAppointmentRepository Appointments { get; }
        ITrainerAvailabilityRepository TrainerAvailabilities { get; }
        IServicesByTrainerRepository ServicesByTrainers { get; }
        IAIDailyPlanRecommendationRepository AIRecommendations { get; }

        // Transaction Methods

        /// <summary>
        /// Tüm değişiklikleri veritabanına kaydet
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Transaction başlat
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Transaction'ı onayla
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Transaction'ı geri al
        /// </summary>
        Task RollbackTransactionAsync();
    }
}

