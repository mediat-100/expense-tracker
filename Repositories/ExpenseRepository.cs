using expense_tracker.Data;
using expense_tracker.Model.Domain;
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

        public async Task<Expense> CreateExpense(Expense expense)
        {
            expense.Id = new Guid();
            await _dbContext.Expenses.AddAsync(expense);
            await _dbContext.SaveChangesAsync();
           
            return expense;
        }

        public async Task<Expense> UpdateExpense(Guid id, Expense expense)
        {
            var existingExpense = await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            if (existingExpense == null) return null;

            existingExpense.Category = expense.Category;
            existingExpense.MerchantName = expense.MerchantName;
            existingExpense.Amount = expense.Amount;

            await _dbContext.SaveChangesAsync();

            return existingExpense;
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
