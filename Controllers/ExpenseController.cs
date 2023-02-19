using expense_tracker.Model;
using expense_tracker.Model.DTO;
using expense_tracker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace expense_tracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController : Controller
    {
        private readonly IExpense _expenseRepository;

        public ExpenseController(IExpense expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllExpensesInDb()
        {
            var expenses = await _expenseRepository.GetAllExpenses();

            return Ok(expenses);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetExpenseById(Guid id)
        {
            var existingExpense = await _expenseRepository.GetExpense(id);

            if (existingExpense == null) return NotFound("Expense id not found");

            return Ok(existingExpense);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDTO request)
        {
            if ((int)request.Category > 2)
            {
                ModelState.AddModelError(nameof(request.Category), "Invalid category");
                return BadRequest(ModelState);
            }

            var newExpense = await _expenseRepository.CreateExpense(request);

            return Ok(newExpense);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateExpenseById(Guid id, UpdateExpenseDTO request)
        {
            if ((int)request.Category <= 0  || (int)request.Category > 3)
            {
                ModelState.AddModelError(nameof(request.Category), "Invalid category");
                return BadRequest(ModelState);
            }

            var updateExpense = await _expenseRepository.UpdateExpense(id, request);

            if (updateExpense == null) return NotFound("Expense id not found");

            return Ok(updateExpense);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteExpenseById(Guid id)
        {
            var deleteExpense = await _expenseRepository.DeleteExpense(id);

            if (deleteExpense == null) return NotFound("Expense id not found");

            return Ok(deleteExpense);
        }

        [HttpGet]
        [Route("total-expenses")]
        public async Task<string> TotalExpenses()
        {
            return await _expenseRepository.TotalExpense();
        }

        [HttpGet]
        [Route("total-expenses/category")]
        public async Task<string> TotalExpensesByCategory(Category category)
        {
            return await _expenseRepository.TotalExpensesByCategory(category);
        }

        [HttpGet]
        [Route("monthly-expenses")]
        public IEnumerable<object> MonthlyExpense()
        {
            return  _expenseRepository.MonthlyExpenses();
        }

        [HttpGet]
        [Route("daily-expenses")]
        public IEnumerable<object> DailyExpense()
        {
            return _expenseRepository.DailyExpenses();
        }

        [HttpGet]
        [Route("weekly-expenses")]
        public IEnumerable<object> WeeklyExpense()
        {
            return _expenseRepository.WeeklyExpenses();
        }

    }
}
