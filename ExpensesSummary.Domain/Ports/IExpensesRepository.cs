using ExpensesSummary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensesSummary.Domain.Ports
{
    public interface IExpensesRepository
    {
        Task<ICollection<Guid>> CreateAsync(ICollection<Expense> expenses);
        Task<IEnumerable<Expense>> GetAllAsync(User user);
        Task<bool> ContainsAsync(double amount, DateTime date);        
        Task<User> GetAsync(string lastname, string firstname);
    }
}
