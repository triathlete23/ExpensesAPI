using ExpensesSummary.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExpensesSummary.Api.Requests
{
    public class GetRequest
    {
        public string UserId { get; set; }
        public string SortOption { get; set; }
    }
}
