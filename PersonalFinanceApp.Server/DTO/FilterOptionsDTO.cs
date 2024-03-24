namespace PersonalFinanceApp.Server.DTO
{
    public class FilterOptionsDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Type { get; set; }
        public string? Category { get; set; }
    }
}
