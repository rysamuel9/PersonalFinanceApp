using Microsoft.AspNetCore.Identity;
using PersonalFinanceApp.Server.Infrastructure;
using PersonalFinanceApp.Server.Models;
using PersonalFinanceApp.Server.Repositories.IRepository;

namespace PersonalFinanceApp.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PasswordHasher<IdentityUser> _passwordHasher;


        public UserService(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _passwordHasher = new PasswordHasher<IdentityUser>();
        }

        public async Task<User> RegisterAsync(string username, string email, string password, string firstName)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists.");
            }

            var identityUser = new IdentityUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create user.");
            }

            var passwordHash = _passwordHasher.HashPassword(identityUser, password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _userRepository.CreateUserAsync(user);

            return user;
        }
    }
}
