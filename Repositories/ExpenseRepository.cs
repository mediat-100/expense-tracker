using expense_tracker.Data;
using expense_tracker.Model.Domain;
using expense_tracker.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace expense_tracker.Repositories
{
    public class ExpenseRepository : IExpense
    {
        private readonly ApplicationDbContext _dbContext;

        public ExpenseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Expense>> GetAllExpenses()
        {
            return await _dbContext.Expenses.ToListAsync();
        }

        public async Task<Expense> GetExpense(Guid id)
        {
            var existingExpense = await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            return existingExpense == null ? null : existingExpense;
        }

        public async Task<ExpenseDTO> CreateExpense(CreateExpenseDTO request)
        {
            var newExpense = new Expense()
            {
                Id = new Guid(),
                ExpenseDate = request.ExpenseDate,
                Amount = request.Amount,
                Category = request.Category,
                MerchantName = request.MerchantName,
            };

            await _dbContext.Expenses.AddAsync(newExpense);
            await _dbContext.SaveChangesAsync();

            // convert to DTO
            var newExpenseDTO = new ExpenseDTO()
            {
                ExpenseDate = newExpense.ExpenseDate,
                Amount = newExpense.Amount,
                MerchantName = newExpense.MerchantName,
                Category = newExpense.Category
            };
           
            return newExpenseDTO;
        }

        public async Task<ExpenseDTO> UpdateExpense(Guid id, UpdateExpenseDTO request)
        {
            var existingExpense = await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            if (existingExpense == null) return null;

            existingExpense.Category = request.Category;
            existingExpense.Amount = request.Amount;

            await _dbContext.SaveChangesAsync();

            // convert to DTO
            var existingExpenseDTO = new ExpenseDTO()
            {
                ExpenseDate = existingExpense.ExpenseDate,
                Amount = existingExpense.Amount,
                MerchantName = existingExpense.MerchantName,
                Category = existingExpense.Category
            };

            return existingExpenseDTO;
        }

        public async Task<Expense> DeleteExpense(Guid id)
        {
            var existingExpense = await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            if (existingExpense == null) return null;

             _dbContext.Expenses.Remove(existingExpense);
            await _dbContext.SaveChangesAsync();
            
            return existingExpense;
        }

    }
}
