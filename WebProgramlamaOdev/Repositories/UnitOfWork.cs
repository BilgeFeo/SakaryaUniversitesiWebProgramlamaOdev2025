using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        private IMemberRepository? _memberRepository;
        private ITrainerRepository? _trainerRepository;
        private IGymRepository? _gymRepository;
        private IServiceTypeRepository? _serviceTypeRepository; 
        private IAppointmentRepository? _appointmentRepository;
        private ITrainerAvailabilityRepository? _trainerAvailabilityRepository;
        private IServicesByTrainerRepository? _servicesByTrainerRepository;
        private IAIDailyPlanRecommendationRepository? _aiRecommendationRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationDbContext Context => _context;

        public IMemberRepository Members
        {
            get
            {
                if (_memberRepository == null)
                {
                    _memberRepository = new MemberRepository(_context);
                }
                return _memberRepository;
            }
        }

        public ITrainerRepository Trainers
        {
            get
            {
                if (_trainerRepository == null)
                {
                    _trainerRepository = new TrainerRepository(_context);
                }
                return _trainerRepository;
            }
        }

        public IGymRepository Gyms
        {
            get
            {
                if (_gymRepository == null)
                {
                    _gymRepository = new GymRepository(_context);
                }
                return _gymRepository;
            }
        }

        // ✅ YENİ EKLEME
        public IServiceTypeRepository ServiceTypes
        {
            get
            {
                if (_serviceTypeRepository == null)
                {
                    _serviceTypeRepository = new ServiceTypeRepository(_context);
                }
                return _serviceTypeRepository;
            }
        }

        public IAppointmentRepository Appointments
        {
            get
            {
                if (_appointmentRepository == null)
                {
                    _appointmentRepository = new AppointmentRepository(_context);
                }
                return _appointmentRepository;
            }
        }

        public ITrainerAvailabilityRepository TrainerAvailabilities
        {
            get
            {
                if (_trainerAvailabilityRepository == null)
                {
                    _trainerAvailabilityRepository = new TrainerAvailabilityRepository(_context);
                }
                return _trainerAvailabilityRepository;
            }
        }

        public IServicesByTrainerRepository ServicesByTrainers
        {
            get
            {
                if (_servicesByTrainerRepository == null)
                {
                    _servicesByTrainerRepository = new ServicesByTrainerRepository(_context);
                }
                return _servicesByTrainerRepository;
            }
        }

        public IAIDailyPlanRecommendationRepository AIRecommendations
        {
            get
            {
                if (_aiRecommendationRepository == null)
                {
                    _aiRecommendationRepository = new AIDailyPlanRecommendationRepository(_context);
                }
                return _aiRecommendationRepository;
            }
        }

        // Transaction Methods
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}