using expense_tracker.Model;
using expense_tracker.Model.Domain;
using expense_tracker.Model.DTO;

namespace expense_tracker.Repositories
{
    public interface IExpense
    {
        Task<IEnumerable<Expense>> GetAllExpenses();
        Task<Expense> GetExpense(Guid id);
        Task<ExpenseDTO> CreateExpense(CreateExpenseDTO expense);
        Task<ExpenseDTO> UpdateExpense(Guid id, UpdateExpenseDTO expense);
        Task<Expense> DeleteExpense(Guid id);
        Task<string> TotalExpense();
        Task<string> TotalExpensesByCategory(Category category);
    }
}
