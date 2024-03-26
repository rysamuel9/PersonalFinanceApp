namespace PersonalFinanceApp.Mobile;

public partial class HomePage : TabbedPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    public HomePage(string token) : this()
    {
        Console.WriteLine($"Token received: {token}");
    }
}