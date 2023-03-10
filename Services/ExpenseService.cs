using expense_tracker.Data;
using expense_tracker.Model;
using expense_tracker.Model.Domain;
using expense_tracker.Model.DTO;
using expense_tracker.Utilities;
using Microsoft.EntityFrameworkCore;

namespace expense_tracker.Repositories
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _dbContext;

        public ExpenseService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Expense>> GetAllExpenses()
        {
            return await _dbContext.Expenses.ToListAsync();
        }

        public async Task<Expense?> GetExpense(Guid id)
        {
            var existingExpense = await _dbContext.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            return existingExpense ?? null;
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

        public async Task<string> TotalExpense()
        {
            var expenses = await _dbContext.Expenses.ToListAsync();

            if (expenses is null)
                return "No expense found";

            var totalExpense = expenses.Sum(x => x.Amount);

            return $"Your total expenses is {totalExpense}";
        }

        public async Task<string> TotalExpensesByCategory(Category category)
        {
            string categoryExpenses;
            var expenses = await _dbContext.Expenses.ToListAsync();

            switch((int) category)
            {
                case 1:
                    var personalExpenses = expenses.FindAll(x => x.Category == 0).Sum(x => x.Amount);
                    categoryExpenses = $"Your total {Category.Personal} expenses is {personalExpenses}";
                    break;
                case 2:
                    var businessExpenses = expenses.FindAll(x => (int) x.Category == 1).Sum(x => x.Amount);
                    categoryExpenses = $"Your total {Category.Business} expenses is {businessExpenses}";
                    break;
                case 3:
                    var socialExpenses = expenses.FindAll(x => (int) x.Category == 2).Sum(x => x.Amount);
                    categoryExpenses = $"Your total {Category.Social} expenses is {socialExpenses}";
                    break;
                default:
                    var totalExpenses = expenses.Sum(x => x.Amount);
                    categoryExpenses = $"Your total expenses is {totalExpenses}";
                    break;
            }

            return categoryExpenses;
        }

        public IEnumerable<object> MonthlyExpenses()
        {
            var monthlyExpense = _dbContext.Expenses.AsEnumerable()
                .GroupBy(x => new { Month = x.ExpenseDate.ToString("MMMM"), x.ExpenseDate.Year },
                (key, group) => new
                {
                    year = key.Year,
                    month = key.Month,
                    totalExpense = group.Sum(y => y.Amount)
                });

            return monthlyExpense;
        }

        public IEnumerable<object> DailyExpenses()
        {
            var dailyExpense = _dbContext.Expenses.AsEnumerable()
                .OrderBy(x => x.ExpenseDate)
                .GroupBy(i => new { day = i.ExpenseDate.DayOfWeek, date = i.ExpenseDate.Date } ,
                (key, group) => new
                {
                    day = key.day.ToString(),
                    key.date,
                    totalExpense = group.Sum(x => x.Amount)
                }
                );

            return dailyExpense;
        }

        public IEnumerable<object> WeeklyExpenses()
        {
            var weeklyExpense = _dbContext.Expenses.AsEnumerable()
               .OrderBy(x => x.ExpenseDate)
               .GroupBy(j => j.ExpenseDate.StartOfWeek(DayOfWeek.Monday),
               (key, group) => new
               {
                   startWeekDate = key.ToString("MM / dd / yyyy"),
                   endWeekDate = key.AddDays(7).ToString("MM / dd / yyyy"),
                   totalExpense = group.Sum(y => y.Amount)
               }
               );

            return weeklyExpense;
        }
    }
}
