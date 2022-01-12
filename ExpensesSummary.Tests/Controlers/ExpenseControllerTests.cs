using ExpensesSummary.Api.Requests;
using ExpensesSummary.Controllers;
using ExpensesSummary.Domain.DomainResult;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
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
            this.controller = new ExpensesController(service.Object);
            this.request = new GetRequest
            {
                User = new User
                {
                    Firstname = "Anthony",
                    Lastname = "Spark"
                }
            };
        }

        [Fact]
        public async void ReturnBadRequestIfGetAllAsyncReturnsAnError()
        {
            var error = "error";
            this.service.Setup(mock => mock.GetAllAsync(It.Is<User>(u => u.Lastname == this.request.User.Lastname), null)).ReturnsAsync(ResultError.WithError(error));

            var response = await this.controller.GetAll(request);

            Assert.Equal(400, ((ObjectResult)response).StatusCode);
        }
    }
}
