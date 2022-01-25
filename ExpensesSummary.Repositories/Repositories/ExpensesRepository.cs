using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using ExpensesSummary.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensesSummary.Repositories.Extensions;

namespace ExpensesSummary.Repositories.Repositories
{
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly ExpenseContext dbContext;

        public ExpensesRepository(ExpenseContext context)
        {
            this.dbContext = context;
        }

        public async Task<Guid> CreateAsync(Expense expense)
        {
            var entity = expense.ToEntity();
            
            await dbContext.Expenses.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            
            return entity.Id;
        }
        
        public async Task<IEnumerable<Expense>> GetAllAsync(Guid userId)
        {
            var dbUser = await dbContext.Users.Include(el => el.Expenses).FirstOrDefaultAsync(el => el.Id == userId);
            var expenses = dbUser.Expenses.Select(el => el.ToDomainModel()).ToArray();
            
            foreach (var expense in expenses)
            {
                expense.User = new User { Firstname = dbUser.Firstname, Lastname = dbUser.Lastname };
            }

            return expenses;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(el => el.Id == userId);
            return user.ToDomainModel();
        }

        public async Task<bool> ContainsAsync(Expense expense)
        {
            return await dbContext.Expenses.AnyAsync(el => 
                el.UserId == expense.UserId && 
                el.Amount == expense.Amount && 
                el.Date == expense.Date);
        }
    }
}
