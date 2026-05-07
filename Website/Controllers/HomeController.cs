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


        public async Task<IActionResult> Index(int? id)
        {
            var userId = 1; ///////// REMEMBER TO CHANGE WHEN LOGIN WORKS
            var boardBelongsToUser = false;

            // fetch the first board belonging to the user
            var firstBoard = await _context.UserHasBoard
               .Where(u => u.UserId == userId && u.Position == 1)
               .Include(u => u.Board)
               .FirstOrDefaultAsync();

            // retrieve the board with the current id and belonging to the currently logged in user
            var isBoardTheirs = await _context.UserHasBoard
                .Include(uhb => uhb.Board)
                .Where(uhb => uhb.UserId == userId && uhb.BoardId == id)
                .FirstOrDefaultAsync();

            // redirect if the user does not have a board
            if (firstBoard == null)
            {
                return RedirectToAction("NoBoards");
            }
            // redirect if no id is present in the URL
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Boards", new { id = firstBoard.BoardId });
            }
            // redirect if the user tries accessing a board they don't own
            if (isBoardTheirs == null)
            {
                return RedirectToAction("Index", "Boards", new { id = firstBoard.BoardId });
            }
            
            return View(isBoardTheirs.Board);
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
