using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ExpensesSummary.Repositories.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Lastname { get; set; }
        
        [Required]
        public string Firstname { get; set; }
        
        [Required]
        public string Currency { get; set; }

        public ICollection<Expense> Expenses { get;set; } = new Collection<Expense>();
    }
}
