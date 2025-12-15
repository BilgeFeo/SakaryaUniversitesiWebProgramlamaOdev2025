using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.Services.ServiceInterfaces;

namespace WebProgramlamaOdev.Services
{


    



    public class AuthService : IAuthService
    {

        private readonly IRepository<ApplicationUser> _userRepository;

        public AuthService (IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<Dictionary<string, ApplicationUser>> GetAllUsersMapAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var userMap = users
                .Where(u => !string.IsNullOrEmpty(u.Email))
                .ToDictionary(u => u.Email!, u => u);

            return userMap;
        }



        public async Task<string> ValidateUser(string LoginAttemptEmail, string LoginAttemptPassword)
        {
            Dictionary<string, ApplicationUser> AllUsersMapWithEmail = await GetAllUsersMapAsync();


            if (AllUsersMapWithEmail.ContainsKey(LoginAttemptEmail))
            {
                 ApplicationUser CurrentAppUser= AllUsersMapWithEmail[LoginAttemptEmail];
                bool isVerified = await Task.Run(() =>BCrypt.Net.BCrypt.Verify(LoginAttemptPassword, CurrentAppUser.PasswordHash));
                if (isVerified)
                {
                  
                    return AllUsersMapWithEmail[LoginAttemptEmail].UserType;
                }
            
                


            }

            return null;


            
        }


        public async Task<bool> ValidateAdmin(string LoginAttemptEmail, string LoginAttemptPassword)
        {
            if (LoginAttemptEmail == "b231210033@sakarya.edu.tr" && LoginAttemptPassword == "sau")
            {
                return true;
            }
            return false;
        }



    }
}
