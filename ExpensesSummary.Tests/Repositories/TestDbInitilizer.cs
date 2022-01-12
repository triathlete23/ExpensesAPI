using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Repositories.Context;
using ExpensesSummary.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesSummary.Tests.Repositories
{
    public static class TestDbInitilizer
    {
        public static ExpenseContext Initialize()
        {
            var options = new DbContextOptionsBuilder<ExpenseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenseContext(options);

            var firstUserId = Guid.NewGuid();
            var secondUserId = Guid.NewGuid();
            context.Users.Add(new User
            {
                Id = firstUserId,
                Currency = Currency.Dollar,
                Firstname = "Anthony",
                Lastname = "Spark"
            });

            context.Expenses.Add(new Expense
            {
                Id = Guid.NewGuid(),
                Currency = Currency.Dollar,
                Nature = "Restaurant",
                Comment = "Comment",
                Amount = 10,
                Date = DateTime.Parse("2021/12/15"),
                UserId = firstUserId
            });

            context.Users.Add(new User
            {
                Id = secondUserId,
                Currency = Currency.Dollar,
                Firstname = "Natasha",
                Lastname = "Romanova"
            });

            context.Expenses.Add(
                new Expense
                {
                    Id = Guid.NewGuid(),
                    Currency = Currency.Rouble,
                    Nature = "Misc",
                    Comment = "Comment",
                    Amount = 49,
                    Date = DateTime.Parse("2021/12/01"),
                    UserId = secondUserId
                });
            context.Expenses.Add(
                new Expense
                {
                    Id = Guid.NewGuid(),
                    Currency = Currency.Rouble,
                    Nature = "Hotel",
                    Comment = "Comment",
                    Amount = 10,
                    Date = DateTime.Parse("2022/01/10"),
                    UserId = secondUserId
                });

            context.SaveChanges();
            return context;
        }
    }
}
