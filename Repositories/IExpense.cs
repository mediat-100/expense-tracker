using expense_tracker.Model.Domain;

namespace expense_tracker.Repositories
{
    public interface IExpense
    {
        Task<IEnumerable<Expense>> GetAllExpenses();
        Task<Expense> GetExpense(Guid id);
        Task<Expense> CreateExpense(Expense expense);
        Task<Expense> UpdateExpense(Guid id, Expense expense);
        Task<Expense> DeleteExpense(Guid id);
    }
}
