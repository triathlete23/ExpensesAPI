using ExpensesSummary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensesSummary.Domain.Ports
{
    public interface IExpensesRepository
    {
        Task<ICollection<Guid>> CreateAsync(ICollection<Expense> expenses);
        Task<IEnumerable<Expense>> GetAllAsync(Guid userId);
        Task<bool> ContainsAsync(double amount, DateTime date);        
        Task<User> GetUserAsync(Guid userId);
    }
}
