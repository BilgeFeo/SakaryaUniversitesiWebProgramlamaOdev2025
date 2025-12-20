using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.Services.ServiceInterfaces;

namespace WebProgramlamaOdev.Services.HomePageServices
{


    



    public class AuthService : IAuthService
    {

        private readonly IRepository<ApplicationUser> _userRepository;

        public AuthService (IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<ApplicationUser> GetUserByEmailAsync(string LoginAttemptEmail)
        {
            var users = await _userRepository.GetAllAsync();

            var userMap = users
                .Where(u => !string.IsNullOrEmpty(u.Email))
                .ToDictionary(u => u.Email!, u => u);
            if (userMap.ContainsKey(LoginAttemptEmail))
            {
                return userMap[LoginAttemptEmail];
            }

            return null;
        }



        public async Task<string> ValidateUser(string LoginAttemptEmail, string LoginAttemptPassword)
        {
           ApplicationUser user = await GetUserByEmailAsync(LoginAttemptEmail);


            if (user !=null)
            {
                 
                bool isVerified = await Task.Run(() =>BCrypt.Net.BCrypt.Verify(LoginAttemptPassword, user.PasswordHash));
                if (isVerified)
                {
                  
                    return user.UserType;
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
