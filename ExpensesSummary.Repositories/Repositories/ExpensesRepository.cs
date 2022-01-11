using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensesSummary.Repositories.Repositories
{
    public class ExpensesRepository : IExpensesRepository
    {
        public async Task<bool> ContainsAsync(double amount, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Guid>> CreateAsync(ICollection<Expense> expenses)
        {
            throw new NotImplementedException();
        }
        
        public async Task<IEnumerable<Expense>> GetAllAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
