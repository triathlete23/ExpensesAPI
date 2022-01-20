using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ExpensesSummary.Tests.Repositories
{
    public class ExpensesRepositoryTests_CreateAsync
    {
        [Fact]
        public async void CreateNewExpensesWithIds()
        {
            using var context = TestDbInitilizer.Initialize();
            var expensesRepository = new ExpensesRepository(context);

            var romanovaId = context.Users.Single(el => el.Lastname == "Romanova").Id;

            var expenseToAdd = new Expense
            {
                Currency = Currency.Rouble,
                Nature = Domain.Enums.Nature.Misc,
                Comment = "Comment",
                Amount = 571,
                Date = DateTime.Parse("2021/12/31"),
                UserId = romanovaId
            };

            var result = await expensesRepository.CreateAsync(expenseToAdd);

            Assert.Equal(context.Expenses.Single(el => el.Amount == expenseToAdd.Amount).Id, result);
            Assert.Equal(3, context.Users.Single(el => el.Id == romanovaId).Expenses.Count);
        }
    }
}
