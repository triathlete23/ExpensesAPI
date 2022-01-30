using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using ExpensesSummary.Domain.Services;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace ExpensesSummary.Tests
{
    public class ExpensesServiceTests_CreateAsync
    {
        private readonly IExpensesService service;

        private readonly Mock<IExpensesRepository> expensesRepository;

        private readonly Expense expense;

        public ExpensesServiceTests_CreateAsync()
        {
            this.expensesRepository = new Mock<IExpensesRepository>();
            this.service = new ExpensesService(this.expensesRepository.Object);

            this.expense = new Expense
            {
                Amount = 66.8,
                Currency = Currency.Dollar,
                Comment = "Some comment",
                Date = DateTime.Parse("01/01/2022"),
                Nature = Domain.Enums.Nature.Restaurant,
                UserId = Guid.NewGuid()
            };            
        }
        
        [Fact]
        public async void ReturnErrorIfDateIsInFuture()
        {
            this.expense.Date = DateTime.Now.AddDays(1);

            var result = await this.service.CreateAsync(this.expense);

            Assert.Equal($"Expense with the amount {this.expense.Amount} and the date {this.expense.Date} must be in the past.", result.Errors.Single());
        }

        [Fact]
        public async void ReturnErrorIfDateIsNotInLastThreeMonths()
        {
            this.expense.Date = DateTime.Now.AddMonths(-4);

            var result = await this.service.CreateAsync(this.expense);

            Assert.Equal($"Expense with the amount {this.expense.Amount} and the date {this.expense.Date} should be done during the last 3 months.", result.Errors.Single());
        }

        [Fact]
        public async void ReturnErrorIfCommentIsEmpty()
        {
            this.expense.Comment = "";

            var result = await this.service.CreateAsync(this.expense);

            Assert.Equal($"Expense with the amount {this.expense.Amount} and the date {this.expense.Date} should contain a comment.", result.Errors.Single());
        }

        [Fact]
        public async void ReturnErrorIfExpenseHasBeenDoneByUnknownUser()
        {
            this.expensesRepository.Setup(mock => mock.GetUserAsync(this.expense.UserId))
                .ReturnsAsync((User)null);

            var result = await this.service.CreateAsync(this.expense);

            Assert.Equal($"Expense with the amount {this.expense.Amount} and for the date {this.expense.Date} has been made by an unknown user.", result.Errors.Single());
        }

        [Fact]
        public async void ReturnErrorIfUserHasAlreadyDeclaredCurrentExpense()
        {
            this.expensesRepository.Setup(mock => mock.GetUserAsync(this.expense.UserId))
                .ReturnsAsync(new User { Id = this.expense.UserId });
            this.expensesRepository.Setup(mock => mock.ContainsAsync(It.Is<Expense>(el =>
                el.UserId == this.expense.UserId &&
                el.Amount == this.expense.Amount &&
                el.Date == this.expense.Date))).ReturnsAsync(true);

            var result = await this.service.CreateAsync(this.expense);

            Assert.Equal($"Expense with the amount {this.expense.Amount} and for the date {this.expense.Date} has been already created.", result.Errors.Single());
        }        

        [Fact]
        public async void ReturnErrorIfExpenseCurrencyDiffersToUserCurrency()
        {
            this.expensesRepository.Setup(mock => mock.GetUserAsync(this.expense.UserId))
                .ReturnsAsync(new User { Currency = Currency.Rouble });

            var result = await this.service.CreateAsync(this.expense);

            Assert.Equal($"Expense with the amount {this.expense.Amount} and for the date {this.expense.Date} has the currency that is different to its user's currency.", result.Errors.Single());
        }

        [Fact]
        public async void CreateAnExpense()
        {
            var id = Guid.NewGuid();
            this.expensesRepository.Setup(mock => mock.GetUserAsync(this.expense.UserId))
                .ReturnsAsync(new User { Currency = Currency.Dollar });
            this.expensesRepository.Setup(mock => mock.CreateAsync(It.Is<Expense>(el =>
                el.Amount == this.expense.Amount &&
                el.Date == this.expense.Date &&
                el.UserId == this.expense.UserId)))
                .ReturnsAsync(id);

            var result = await this.service.CreateAsync(this.expense);

            Assert.False(result.HasError);
            Assert.Equal(id, result.Data);
        }
    }
}
