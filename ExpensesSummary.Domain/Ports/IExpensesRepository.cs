using ExpensesSummary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensesSummary.Domain.Ports
{
    public interface IExpensesRepository
    {
        Task<Guid> CreateAsync(Expense expense);
        Task<IEnumerable<Expense>> GetAllAsync(Guid userId);
        Task<User> GetUserAsync(Guid userId);
        Task<bool> ContainsAsync(Expense expense);
    }
}
