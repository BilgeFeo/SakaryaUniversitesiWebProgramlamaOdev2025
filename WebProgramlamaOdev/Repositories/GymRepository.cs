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

     
        public override async Task<Gym?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(g => g.User)  
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        
        public override async Task<IEnumerable<Gym>> GetAllAsync()
        {
            return await _dbSet
                .Include(g => g.User)  
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Gym gym)
        {

            try
            {
                // Kullanıcıyı bellek üzerindeki takip listesine ekler
                await _context.Gyms.AddAsync(gym);

                // Değişiklikleri veritabanına yazar
                // SaveChangesAsync, etkilenen satır sayısını döner. 0'dan büyükse işlem başarılıdır.
                int result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch
            {
                // Herhangi bir hata oluşursa (örn: veritabanı bağlantısı koparsa) false döner
                return false;
            }
        }
       
        public override async Task<IEnumerable<Gym>> GetWhereAsync(Expression<Func<Gym, bool>> predicate)
        {
            return await _dbSet
                .Include(g => g.User)  
                .Where(predicate)
                .ToListAsync();
        }

       
        public async Task<IEnumerable<Gym>> GetActiveGymsAsync()
        {
            return await _dbSet
                .Include(g => g.User)  
                .Where(g => g.IsActive)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        
        public async Task<Gym?> GetWithServicesAsync(int gymId)
        {
            return await _dbSet
                .Include(g => g.User)  
                .Include(g => g.ServiceList)
                .FirstOrDefaultAsync(g => g.Id == gymId);
        }

        public async Task<bool> IsGymActiveAsync(int gymId)
        {
            var gym = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == gymId);

            return gym != null && gym.IsActive;
        }

        
        public async Task<Gym?> GetWithTrainersAsync(int gymId)
        {
            return await _dbSet
                .Include(g => g.User)  
                .Include(g => g.TrainerList)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(g => g.Id == gymId);
        }

        
        public async Task<Gym?> GetWithServicesAndTrainersAsync(int gymId)
        {
            return await _dbSet
                .Include(g => g.User)  
                .Include(g => g.ServiceList)
                .Include(g => g.TrainerList)
                    .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(g => g.Id == gymId);
        }

        
        public async Task<IEnumerable<Gym>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Include(g => g.User)  
                .Where(g => g.Name.Contains(name))
                .OrderBy(g => g.Name)
                .ToListAsync();
        }
    }
}