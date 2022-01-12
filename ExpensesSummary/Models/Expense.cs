using ExpensesSummary.Domain.Enums;
using System;

namespace ExpensesSummary.Api.Models
{
    public class Expense
    {
        public string User { get; set; }
        public DateTime Date { get; set; }
        public string Nature { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Comment { get; set; }
    }
}
