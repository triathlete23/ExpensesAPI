using ExpensesSummary.Domain.Models;
using ExpensesSummary.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ExpensesSummary.Tests.Repositories
{
    public class ExpensesRepositoryTests_GetAllAsync
    {
        [Fact]
        public async void ReturnAllExpensesForCurrentUser()
        {
            using var context = TestDbInitilizer.Initialize();
            var expensesRepository = new ExpensesRepository(context);

            var tonySparkId = context.Users.Single(el => el.Lastname == "Spark" && el.Firstname == "Anthony").Id;

            var result = await expensesRepository.GetAllAsync(tonySparkId);

            Assert.Single(result);
            Assert.Equal(10, result.Single().Amount);
            Assert.Equal(DateTime.Parse("2021/12/15"), result.Single().Date);
        }
    }
}
