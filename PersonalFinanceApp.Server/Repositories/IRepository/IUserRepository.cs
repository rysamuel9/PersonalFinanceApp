using PersonalFinanceApp.Server.Models;

namespace PersonalFinanceApp.Server.Repositories.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
    }
}
