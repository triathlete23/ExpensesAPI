using System;

namespace ExpensesSummary.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Currency { get; set; }
    }
}
