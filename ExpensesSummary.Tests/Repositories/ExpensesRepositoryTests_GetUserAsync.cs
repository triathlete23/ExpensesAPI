using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ExpensesSummary.Tests.Repositories
{
    public class ExpensesRepositoryTests_GetUserAsync
    {
        [Fact]
        public async void ReturnCurrentUser()
        {
            using var context = TestDbInitilizer.Initialize();
            var expensesRepository = new ExpensesRepository(context);

            var tonySparkId = context.Users.Single(el => el.Lastname == "Spark" && el.Firstname == "Anthony").Id;

            var user = await expensesRepository.GetUserAsync(tonySparkId);
            Assert.NotNull(user);
            Assert.Equal(Currency.Dollar, user.Currency);
            Assert.Equal("Anthony", user.Firstname);
            Assert.Equal("Spark", user.Lastname);
            Assert.False(user.Expenses.Any());
        }

        [Fact]
        public async void ReturnNullForUnexistingUser()
        {
            using var context = TestDbInitilizer.Initialize();
            var expensesRepository = new ExpensesRepository(context);

            Assert.Null(await expensesRepository.GetUserAsync(Guid.NewGuid()));
        }
    }
}
