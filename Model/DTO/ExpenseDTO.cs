namespace expense_tracker.Model.DTO
{
    public class ExpenseDTO
    {
        public DateTime ExpenseDate { get; set; }
        public string? MerchantName { get; set; }
        public int Amount { get; set; }
        public Classification Category { get; set; }
    }
}
