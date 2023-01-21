namespace expense_tracker.Model.Domain
{
    public class Expense
    {
        public Guid Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? MerchantName { get; set; }
        public int Amount { get; set; }
        public Classification Category { get; set; }
    }
}
