using ExpensesSummary.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesSummary.Repositories.Context
{
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    }
}
