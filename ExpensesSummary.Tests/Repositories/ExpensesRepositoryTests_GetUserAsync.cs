using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Repositories.Repositories;
using System;
using System.Collections.Generic;
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

            var user = await expensesRepository.GetUserAsync("Spark", "Anthony");
            Assert.NotNull(user);
            Assert.Equal(Currency.Dollar, user.Currency);
            Assert.Equal("Anthony", user.Firstname);
            Assert.Equal("Spark", user.Lastname);
        }

        [Fact]
        public async void ReturnNullForUnexistingUser()
        {
            using var context = TestDbInitilizer.Initialize();
            var expensesRepository = new ExpensesRepository(context);

            Assert.Null( await expensesRepository.GetUserAsync("Liahushyn", "Pavlo"));
        }
    }
}
