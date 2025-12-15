using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.Services.ServiceInterfaces;

namespace WebProgramlamaOdev.Services
{
    public class SignUpService : ISignUpService
    {

        private readonly IApplicationUserRepository _ApplicationUserRepository;
        private readonly IMemberRepository _MemberRepository;

        public SignUpService(IApplicationUserRepository ApplicationUserRepository, IMemberRepository MemberRepository)
        {
            _MemberRepository = MemberRepository;
            _ApplicationUserRepository = ApplicationUserRepository;
        
        }

        public async Task<bool> RegisterMemberAsync(MemberRegisterRequestDto registerRequestDto)
        {

         


            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = registerRequestDto.Email,
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName,
                UserType = "Member",
                PasswordHash=BCrypt.Net.BCrypt.HashPassword(registerRequestDto.Password),
                PhoneNumber = registerRequestDto.PhoneNumber,
            };

            if (!await _ApplicationUserRepository.AddAsync(newUser))
            {
                return false;
            }


            var newMemberProfile = new Member
            {
                // ApplicationUser'dan gelen Id ile birebir ilişki kurulur.
                UserId = newUser.Id,

                DateOfBirth = registerRequestDto.DateOfBirth,
                Gender = registerRequestDto.Gender,
                Height = registerRequestDto.Height,
                Weight = registerRequestDto.Weight,

                
            };

            if (!await _MemberRepository.AddAsync(newMemberProfile))
            {
                
                return false;
            }

            return true;
        }







    }
}
