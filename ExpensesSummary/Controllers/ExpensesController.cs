using ExpensesSummary.Api.Extensions;
using ExpensesSummary.Api.Requests;
using ExpensesSummary.Domain.Models;
using ExpensesSummary.Domain.Ports;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesSummary.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesService expensesService;

        public ExpensesController(IExpensesService expensesService)
        {
            this.expensesService = expensesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromBody] GetRequest request)
        {
            var serviceResult = await expensesService.GetAllAsync(request.UserId, request.SortOption);
            if (serviceResult.HasError)
            {
                return BadRequest(string.Join(' ', serviceResult.Errors));
            }

            return Ok(serviceResult.Data.Select(el => el.ToApiResponse()).ToArray());
        }

        [HttpPost]        
        public async Task<IActionResult> CreateAsync([FromBody] Expense expense)
        {
            var serviceResult = await expensesService.CreateAsync(expense);
            if (serviceResult.HasError)
            {
                return BadRequest(string.Join(' ', serviceResult.Errors));
            }

            return Ok(serviceResult.Data);
        }
    }
}
