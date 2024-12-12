namespace API.DTOs
{
    public class AdminRevenueCompiledDTO
    {
        public List<AdminRevenueDTO> RevenueDetails { get; set; }
        public int TotalRevenue { get; set; }
    }
}
