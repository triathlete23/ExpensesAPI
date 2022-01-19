using ExpensesSummary.Domain.Models;
using System;

namespace ExpensesSummary.Api.Requests
{
    public class GetRequest
    {
        public string UserId { get; set; }
        public string SortOption { get; set; }
    }
}
