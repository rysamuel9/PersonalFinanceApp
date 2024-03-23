namespace PersonalFinanceApp.Server.DTO
{
    public class AccountRegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
    }
}
