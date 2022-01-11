using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ExpensesSummary.Repositories.Models
{
    public class Expense
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Nature { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Comment { get; set; }
        
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
    }
}
