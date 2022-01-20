using ExpensesSummary.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExpensesSummary.Domain.Models
{
    public class Expense
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public Nature Nature { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }        
        public string Comment { get; set; }
        public Guid UserId { get; set; }
    }
}
