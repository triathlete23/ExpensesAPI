using ExpensesSummary.Domain.DomainResult;
using ExpensesSummary.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensesSummary.Domain.Ports
{
    public interface IExpensesService
    {
        public Task<Result<ICollection<Guid>>> CreateAsync(ICollection<Expense> expenses);
        public Task<Result<IEnumerable<Expense>>> GetAllAsync(string userId, string sortOption = null);
    }
}
