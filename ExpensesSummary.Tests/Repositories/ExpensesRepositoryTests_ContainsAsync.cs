using System;
using ExpensesSummary.Repositories.Repositories;
using System.Linq;
using Xunit;

namespace ExpensesSummary.Tests.Repositories
{
    public class ExpensesRepositoryTests_ContainsAsync
    {
        [Fact]
        public async void ReturnTrueIfExpenseExists()
        {
            using var context = TestDbInitilizer.Initialize();
            var expensesRepository = new ExpensesRepository(context);
            var expenseToFind = context.Expenses.FirstOrDefault();

            Assert.True(await expensesRepository.ContainsAsync(new Domain.Models.Expense {                 
                UserId = expenseToFind.UserId, 
                Amount = expenseToFind.Amount, 
                Date = expenseToFind.Date 
            }));
        }

        [Fact]
        public async void ReturnFalseIfExpenseDoesntExist()
        {
            using var context = TestDbInitilizer.Initialize();
            var expensesRepository = new ExpensesRepository(context);

            Assert.False(await expensesRepository.ContainsAsync(new Domain.Models.Expense
            {
                UserId = Guid.NewGuid(),
                Amount = 100,
                Date = DateTime.Now
            }));
        }
    }
}
