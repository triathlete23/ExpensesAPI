using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using ExpensesSummary.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExpensesSummary.Tests
{
    public class ExpensesServiceTests_GetAllAsync
    {
        private readonly IExpensesService service;

        private readonly Mock<IExpensesRepository> expensesRepository;

        private readonly Guid userId = Guid.NewGuid();
        private readonly ICollection<Expense> expenses;

        public ExpensesServiceTests_GetAllAsync()
        {
            this.expensesRepository = new Mock<IExpensesRepository>();
            this.service = new ExpensesService(this.expensesRepository.Object);

            this.expenses = new List<Expense>
            {
                new Expense
                {
                    Amount = 66.8,
                    Currency = Currency.Rouble,
                    Comment = "Some comment",
                    Date = DateTime.Parse("13/12/2021"),
                    Nature = Domain.Enums.Nature.Restaurant,
                    UserId = this.userId
                },
                new Expense
                {
                    Amount = 20,
                    Currency = Currency.Rouble,
                    Comment = "Some comment",
                    Date = DateTime.Parse("08/01/2022"),
                    Nature = Domain.Enums.Nature.Misc,
                    UserId = this.userId
                },
                new Expense
                {
                    Amount = 148.8,
                    Currency = Currency.Rouble,
                    Comment = "Another comment",
                    Date = DateTime.Parse("07/01/2022"),
                    Nature = Domain.Enums.Nature.Hotel,
                    UserId = this.userId
                }
            };
        }

        [Fact]
        public async void ReturnErrorIfUserIdIsEmpty()
        {
            var result = await this.service.GetAllAsync("");

            Assert.Equal("UserId cannot be empty.", result.Error);
        }

        [Fact]
        public async void ReturnErrorIfUserIdCannotBeParsed()
        {
            var result = await this.service.GetAllAsync("invalid");

            Assert.Equal("UserId has an incorrect format.", result.Error);
        }

        [Fact]
        public async void ReturnErrorIfThereAreNoExpensesForUser()
        {
            var result = await this.service.GetAllAsync(Guid.NewGuid().ToString());

            Assert.Equal("There are no expenses for current user.", result.Error);
        }

        [Fact]
        public async Task GetAllSortedByAmount()
        {
            this.expensesRepository.Setup(mock => mock.GetAllAsync(this.userId))
                .ReturnsAsync(expenses);

            var result = await this.service.GetAllAsync(this.userId.ToString());

            Assert.Equal(20, result.Data.ElementAt(0).Amount);
            Assert.Equal(66.8, result.Data.ElementAt(1).Amount);
            Assert.Equal(148.8, result.Data.ElementAt(2).Amount);
        }

        [Fact]
        public async Task GetAllSortedByDate()
        {
            this.expensesRepository.Setup(mock => mock.GetAllAsync(this.userId))
                .ReturnsAsync(expenses);

            var result = await this.service.GetAllAsync(this.userId.ToString(), "date");

            Assert.Equal(DateTime.Parse("13/12/2021"), result.Data.ElementAt(0).Date);
            Assert.Equal(DateTime.Parse("07/01/2022"), result.Data.ElementAt(1).Date);
            Assert.Equal(DateTime.Parse("08/01/2022"), result.Data.ElementAt(2).Date);
        }
    }
}
