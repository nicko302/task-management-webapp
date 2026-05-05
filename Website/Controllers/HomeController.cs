using CRUD_Application.Data;
using CRUD_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace CRUD_Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var userId = 1; ///////// REMEMBER TO CHANGE WHEN LOGIN WORKS

            // fetch the first board belonging to the user
            var firstBoard = await _context.UserHasBoard
               .Where(u => u.UserId == userId && u.Position == 1)
               .Include(u => u.Board)
               .FirstOrDefaultAsync();

            if (firstBoard != null)
            {
                return RedirectToAction("Index", "Boards", new { id = firstBoard.BoardId });
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
