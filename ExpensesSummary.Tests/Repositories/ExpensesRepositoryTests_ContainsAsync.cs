using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Domain.Ports;
using ExpensesSummary.Repositories.Context;
using ExpensesSummary.Repositories.Models;
using ExpensesSummary.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ExpensesSummary.Tests.Repositories
{
    public class ExpensesRepositoryTests_ContainsAsync
    {
        [Fact]
        public async void ReturnFalseIfThereIsNoExpenseWithAmountAndDate()
        {
            using var context = TestDbInitilizer.Initialize();
            var expensesRepository = new ExpensesRepository(context);

            Assert.False(await expensesRepository.ContainsAsync(15, DateTime.Now));
        }

        [Fact]
        public async void ReturnTrueIfThereIsNoExpenseWithAmountAndDate()
        {
            using var context = TestDbInitilizer.Initialize();
            var expensesRepository = new ExpensesRepository(context);
            Assert.True(await expensesRepository.ContainsAsync(10, DateTime.Parse("2021/12/15")));
        }
    }
}
