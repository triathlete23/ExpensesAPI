using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Models.Validators;
using ExpensesSummary.Domain.Ports;
using FluentValidation.Results;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace ExpensesSummary.Tests.Validators
{
    public class ExpenseValidatorTests
    {
        private readonly Mock<IExpensesRepository> expensesRepository;
        private readonly ExpenseValidator validator;
        private readonly Expense expense;

        public ExpenseValidatorTests()
        {
            this.expensesRepository = new Mock<IExpensesRepository>();
            this.validator = new ExpenseValidator(this.expensesRepository.Object);

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

            var result = await this.validator.ValidateAsync(this.expense);

            Assert.False(result.IsValid);
            Assert.Equal("Expense should be done during the last 3 months.", result.Errors.Single().ErrorMessage);
        }

        [Fact]
        public async void ReturnErrorIfDateIsNotInLastThreeMonths()
        {
            this.expense.Date = DateTime.Now.AddMonths(-4);

            var result = await this.validator.ValidateAsync(this.expense);

            Assert.False(result.IsValid);
            Assert.Equal("Expense should be done during the last 3 months.", result.Errors.Single().ErrorMessage);
        }

        [Fact]
        public async void ReturnErrorIfCommentIsEmpty()
        {
            this.expense.Comment = "";

            var result = await this.validator.ValidateAsync(this.expense);

            Assert.False(result.IsValid);
            Assert.Equal("Expense should contain a comment.", result.Errors.Single().ErrorMessage);
        }

        [Fact]
        public async void ReturnErrorIfAmountIsANegativeNumber()
        {
            this.expense.Amount = -39;

            var result = await this.validator.ValidateAsync(this.expense);

            Assert.False(result.IsValid);
            Assert.Equal("Expense's amount should be a positive number.", result.Errors.Single().ErrorMessage);
        }

        [Fact]
        public async void ReturnErrorIfExpenseHasBeenDoneByUnknownUser()
        {
            this.expensesRepository.Setup(mock => mock.GetUserAsync(this.expense.UserId))
                .ReturnsAsync((User)null);

            var result = await this.validator.ValidateAsync(this.expense);

            Assert.False(result.IsValid);
            Assert.Equal("Expense's user is unknown or expense's currency is different to user's currency.", result.Errors.Single().ErrorMessage);
        }

        [Fact]
        public async void ReturnErrorIfExpenseCurrencyDiffersToUserCurrency()
        {
            this.expensesRepository.Setup(mock => mock.GetUserAsync(this.expense.UserId))
                .ReturnsAsync(new User { Currency = Currency.Rouble });

            var result = await this.validator.ValidateAsync(this.expense);

            Assert.False(result.IsValid);
            Assert.Equal("Expense's user is unknown or expense's currency is different to user's currency.", result.Errors.Single().ErrorMessage);
        }

        [Fact]
        public async void ReturnErrorIfUserHasAlreadyDeclaredCurrentExpense()
        {
            this.expensesRepository.Setup(mock => mock.GetUserAsync(this.expense.UserId))
                .ReturnsAsync(new User { Id = this.expense.UserId, Currency = Currency.Dollar });
            this.expensesRepository.Setup(mock => mock.ContainsAsync(It.Is<Expense>(el =>
                el.UserId == this.expense.UserId &&
                el.Amount == this.expense.Amount &&
                el.Date == this.expense.Date))).ReturnsAsync(true);

            var result = await this.validator.ValidateAsync(this.expense);

            Assert.False(result.IsValid);
            Assert.Equal("Expense with same date and amount has been already created.", result.Errors.Single().ErrorMessage);
        }
        
        [Fact]
        public async void ValidateExpense()
        {
            this.expensesRepository.Setup(mock => mock.GetUserAsync(this.expense.UserId))
                .ReturnsAsync(new User { Id = this.expense.UserId, Currency = Currency.Dollar });
            this.expensesRepository.Setup(mock => mock.ContainsAsync(It.Is<Expense>(el =>
                el.UserId == this.expense.UserId &&
                el.Amount == this.expense.Amount &&
                el.Date == this.expense.Date))).ReturnsAsync(false);

            var result = await this.validator.ValidateAsync(this.expense);

            Assert.True(result.IsValid);
        }
    }
}
