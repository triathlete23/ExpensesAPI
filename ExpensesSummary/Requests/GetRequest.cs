using ExpensesSummary.Domain.Models;

namespace ExpensesSummary.Api.Requests
{
    public class GetRequest
    {
        public User User { get; set; }
        public string SortOption { get; set; }
    }
}
