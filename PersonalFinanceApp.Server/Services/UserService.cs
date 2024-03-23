using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceApp.Server.Infrastructure;
using PersonalFinanceApp.Server.Models;
using PersonalFinanceApp.Server.Repositories.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalFinanceApp.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PasswordHasher<IdentityUser> _passwordHasher;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserService(IUserRepository userRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _passwordHasher = new PasswordHasher<IdentityUser>();
            _signInManager = signInManager;
        }


        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid password.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new byte[32];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
        new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
