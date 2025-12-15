using System;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace WebProgramlamaOdev.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {

        
    private readonly ApplicationDbContext _context; // Veritabanı context'iniz (Adı projenize göre değişebilir)

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. E-posta adresine göre kullanıcı getiren fonksiyon
        // Kullanım: await _userRepository.GetByEmailAsync(dto.Email);
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            // Veritabanında email eşleşmesi arar. Bulursa kullanıcıyı, bulamazsa null döner.
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Email == email);
        }

        // 2. Yeni kullanıcı ekleyen fonksiyon
        // Kullanım: if (!await _userRepository.AddAsync(newUser)) { ...hata... }
        public async Task<bool> AddAsync(ApplicationUser user)
        {
            try
            {
                // Kullanıcıyı bellek üzerindeki takip listesine ekler
                await _context.Users.AddAsync(user);

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
    }



}

