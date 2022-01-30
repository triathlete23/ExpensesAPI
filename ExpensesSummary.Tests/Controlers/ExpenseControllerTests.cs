using ExpensesSummary.Api.Models;
using ExpensesSummary.Api.Requests;
using ExpensesSummary.Controllers;
using ExpensesSummary.Domain.Constants;
using ExpensesSummary.Domain.DomainResult;
using ExpensesSummary.Domain.Ports;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ExpensesSummary.Tests.Controlers
{
    public class ExpenseControllerTests
    {
        private readonly ExpensesController controller;
        private readonly Mock<IExpensesService> service;
        private readonly GetRequest request;
        public ExpenseControllerTests()
        {
            this.service = new Mock<IExpensesService>();
            this.controller = new ExpensesController(service.Object);
            this.request = new GetRequest
            {
                UserId = Guid.NewGuid().ToString()
            };
        }

        [Fact]
        public async void ReturnBadRequestIfGetAllAsyncReturnsAnError()
        {
            var error = "error";
            this.service.Setup(mock => mock.GetAllAsync(this.request.UserId, null)).ReturnsAsync(ResultError.WithErrors(error));

            var result = await this.controller.GetAllAsync(request);

            Assert.Equal(400, ((ObjectResult)result).StatusCode);
            Assert.Equal(error, ((ObjectResult)result).Value);
        }

        [Fact]
        public async void ReturnOKIfGetAllAsyncReturnsData()
        {
            this.service.Setup(mock => mock.GetAllAsync(this.request.UserId, null))
                .ReturnsAsync(Result<IEnumerable<Domain.Models.Expense>>.WithData(new Domain.Models.Expense[] {
                    new Domain.Models.Expense
                    {
                        Currency = Currency.Dollar,
                        Nature = Domain.Enums.Nature.Restaurant,
                        Comment = "Comment",
                        Amount = 10,
                        Date = DateTime.Parse("2021/12/15"),
                        User = new Domain.Models.User{ Currency = Currency.Dollar, Firstname = "Romanova", Lastname = "Natasha"}
                    }
                }));

            var result = await this.controller.GetAllAsync(request);

            Assert.Equal(200, ((ObjectResult)result).StatusCode);
            Assert.Single((IEnumerable<Expense>)((ObjectResult)result).Value);
        }

        [Fact]
        public async void ReturnBadRequestIfCreateAsyncReturnsAnError()
        {
            var error = "error";
            this.service.Setup(mock => mock.CreateAsync(It.IsAny<Domain.Models.Expense>())).ReturnsAsync(ResultError.WithErrors(error));
            var result = await this.controller.CreateAsync(new Domain.Models.Expense());

            Assert.Equal(400, ((ObjectResult)result).StatusCode);
            Assert.Equal(error, ((ObjectResult)result).Value);
        }

        [Fact]
        public async void ReturnOKIfCreateAsyncReturnsData()
        {
            var id = Guid.NewGuid();
            this.service.Setup(mock => mock.CreateAsync(It.IsAny<Domain.Models.Expense>())).ReturnsAsync(
                Result<Guid>.WithData(id)
                );
            var result = await this.controller.CreateAsync(new Domain.Models.Expense());

            Assert.Equal(200, ((ObjectResult)result).StatusCode);
            Assert.Equal(id, (Guid)((ObjectResult)result).Value);
        }
    }
}
