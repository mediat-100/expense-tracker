using Microsoft.AspNetCore.Mvc;

namespace expense_tracker.Controllers
{
    public class ExpenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
