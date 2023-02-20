namespace expense_tracker.Utilities
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            var dateAdded = dt.AddDays(-1 * diff).Date;
            return dateAdded;
        }
    }
}
