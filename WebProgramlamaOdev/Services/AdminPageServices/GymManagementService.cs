using Microsoft.AspNetCore.Identity;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;

namespace WebProgramlamaOdev.Services.AdminPageServices
{
    public class GymManagementService :IGymManagementService
    {

        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IGymRepository _gymRepository;


        public GymManagementService(IApplicationUserRepository applicationUserRepository, IGymRepository gymRepository)
        {

            _applicationUserRepository = applicationUserRepository;
            _gymRepository = gymRepository;

        }

        
        public async Task<bool> CreateGymAndSaveOnDatabase(CreateGymRequestDto createGymRequestDto)
        {

            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = createGymRequestDto.Email,
                FirstName = "Gym",
                LastName = "Salon",
                UserType = "Gym",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createGymRequestDto.Password),
                PhoneNumber = createGymRequestDto.PhoneNumber,
            };

            if (!await _applicationUserRepository.AddAsync(newUser))
            {
                return false;
            }

            var newGym = new Gym
            {
                UserId = newUser.Id,
                Name = createGymRequestDto.Name,
                Address = createGymRequestDto.Address,
                OpeningTime = createGymRequestDto.OpeningTime,
                ClosingTime = createGymRequestDto.ClosingTime,
                IsActive =true
            };
            if (!await _gymRepository.AddAsync(newGym))
            {
                return false;
            }

            return true;
        } 






    }
}
