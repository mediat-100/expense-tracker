﻿using expense_tracker.Data;
using expense_tracker.Model;
using expense_tracker.Model.Domain;
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
        public async Task<IActionResult> CreateExpense([FromBody]Expense expense)
        {
            var newExpense = await _expenseRepository.CreateExpense(expense);

            return Ok(newExpense);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateExpenseById(Guid id, Expense expense)
        {
            var updateExpense = await _expenseRepository.UpdateExpense(id, expense);

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
    }
}
