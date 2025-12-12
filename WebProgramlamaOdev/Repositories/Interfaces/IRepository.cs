using System.Linq.Expressions;


namespace WebProgramlamaOdev.Repositories.Interfaces
{
    
    public interface IRepository<T> where T : class
    {
        

        //Get Operasyonlari
        
        
        // ID'ye göre entity getir
        Task<T?> GetByIdAsync(int id);

        // ID'ye göre entity getir (string Id için - ApplicationUser)
        Task<T?> GetByIdAsync(string id);

        // Tüm entity'leri getir
        Task<IEnumerable<T>> GetAllAsync();

        // Şarta göre entity'leri getir
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        // Şarta göre tek entity getir
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // Şarta göre tek entity getir (bulamazsa exception fırlat)
        Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate);

        // Sayfalama ile entity'leri getir
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);

        // Şarta göre sayfalama ile entity'leri getir
        Task<IEnumerable<T>> GetPagedAsync(
            Expression<Func<T, bool>> predicate,
            int pageNumber,
            int pageSize
        );

        
        // Create Operasyonlari
        

        // Yeni entity ekle
        Task<T> AddAsync(T entity);

        // Birden fazla entity ekle
        Task AddRangeAsync(IEnumerable<T> entities);

        
        // Update Operasyonlari
        

        // Entity güncelle
        Task UpdateAsync(T entity);

        // Birden fazla entity güncelle
        Task UpdateRangeAsync(IEnumerable<T> entities);

        
        // Deltee Operasyonlari
        

        // Entity sil
        Task DeleteAsync(T entity);

        // ID'ye göre entity sil
        Task DeleteByIdAsync(int id);

        // ID'ye göre entity sil (string Id için)
        Task DeleteByIdAsync(string id);

        // Birden fazla entity sil
        Task DeleteRangeAsync(IEnumerable<T> entities);

        // Şarta göre entity'leri sil
        Task DeleteWhereAsync(Expression<Func<T, bool>> predicate);

        
        // Query Operasyonlari
        

        /// Şarta uyan entity var mı kontrol et
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        // Herhangi bir entity var mı kontrol et
        Task<bool> AnyAsync();

        // Şarta uyan entity sayısı
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        // Toplam entity sayısı
        Task<int> CountAsync();

        // Include Operasyonlari (Navigation Properties)
        

        // Navigation property'leri dahil et
        Task<IEnumerable<T>> GetAllWithIncludesAsync(
            params Expression<Func<T, object>>[] includes
        );

        // Şarta göre navigation property'leri dahil et
        Task<IEnumerable<T>> GetWhereWithIncludesAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes
        );

        // ID'ye göre navigation property'leri dahil et
        Task<T?> GetByIdWithIncludesAsync(
            int id,
            params Expression<Func<T, object>>[] includes
        );
    }
}