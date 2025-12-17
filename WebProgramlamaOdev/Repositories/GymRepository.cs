using Microsoft.EntityFrameworkCore;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using System.Linq.Expressions;

namespace WebProgramlamaOdev.Repositories
{
    public class GymRepository : Repository<Gym>, IGymRepository
    {
        public GymRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Override - Id'ye göre gym getir (User bilgisiyle)
        public override async Task<Gym?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(g => g.User)
                .Include(g => g.ServiceList)
                .Include(g => g.TrainerList)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        // Override - Tüm gym'leri getir (User, Service, Trainer bilgileriyle)
        public override async Task<IEnumerable<Gym>> GetAllAsync()
        {
            return await _dbSet
                .Include(g => g.User)
                .Include(g => g.ServiceList)
                .Include(g => g.TrainerList)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        // Gym ekle
        public async Task<bool> AddAsync(Gym gym)
        {
            try
            {
                await _context.Gyms.AddAsync(gym);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        // Override - Koşula göre gym'leri getir
        public override async Task<IEnumerable<Gym>> GetWhereAsync(Expression<Func<Gym, bool>> predicate)
        {
            return await _dbSet
                .Include(g => g.User)
                .Include(g => g.ServiceList)
                .Include(g => g.TrainerList)
                .Where(predicate)
                .ToListAsync();
        }

        // Aktif gym'leri getir
        public async Task<IEnumerable<Gym>> GetActiveGymsAsync()
        {
            return await _dbSet
                .Include(g => g.User)
                .Include(g => g.ServiceList)
                .Include(g => g.TrainerList)
                .Where(g => g.IsActive)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        // Gym'i servisleriyle getir
        public async Task<Gym?> GetWithServicesAsync(int gymId)
        {
            return await _dbSet
                .Include(g => g.User)
                .Include(g => g.ServiceList)
                .FirstOrDefaultAsync(g => g.Id == gymId);
        }

        // Gym aktif mi kontrol et
        public async Task<bool> IsGymActiveAsync(int gymId)
        {
            var gym = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == gymId);

            return gym != null && gym.IsActive;
        }

        // Gym'i antrenörleriyle getir
        public async Task<Gym?> GetWithTrainersAsync(int gymId)
        {
            return await _dbSet
                .Include(g => g.User)
                .Include(g => g.TrainerList)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(g => g.Id == gymId);
        }

        // Gym'i hem servisler hem antrenörlerle getir
        public async Task<Gym?> GetWithServicesAndTrainersAsync(int gymId)
        {
            return await _dbSet
                .Include(g => g.User)
                .Include(g => g.ServiceList)
                .Include(g => g.TrainerList)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(g => g.Id == gymId);
        }

        // İsme göre arama
        public async Task<IEnumerable<Gym>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Include(g => g.User)
                .Include(g => g.ServiceList)
                .Include(g => g.TrainerList)
                .Where(g => g.Name.Contains(name))
                .OrderBy(g => g.Name)
                .ToListAsync();
        }
    }
}