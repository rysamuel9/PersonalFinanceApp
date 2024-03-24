using PersonalFinanceApp.Server.Models;

namespace PersonalFinanceApp.Server.Repositories.IRepository
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string email, string password, string firstName);
        Task<string> LoginAsync(string username, string password);
    }
}
