using CRUD_Application.Data;
using CRUD_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
namespace CRUD_Application.Controllers
{
    public class BoardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BoardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Boards
        [HttpGet("/Boards/Index/{id?}")]
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                var board = await _context.Board
                    .Include(b => b.Lists.OrderBy(l => l.Position))
                        .ThenInclude(l => l.Tasks.OrderBy(t => t.Position))
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (board != null)
                {
                    return View("Index", board);
                }
            }

            return View("Index", null);
        }

        

        /** 
        // GET: Board/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Board/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Board board)
        {
            if (ModelState.IsValid)
            {
                _context.Board.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }
        // GET: Boards/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var board = await _context.Board.FindAsync(id);
            if (board == null) return NotFound();
            return View(board);
        }
        // POST: Boards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Board board)
        {
            if (id != board.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Board.Update(board);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }
        // GET: Boards/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var board = await _context.Board
            .FirstOrDefaultAsync(c => c.Id == id);
            if (board == null) return NotFound();
            return View(board);
        }
        // GET: Boards/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var board = await _context.Board
            .FirstOrDefaultAsync(c => c.Id == id);
            if (board == null) return NotFound();
            return View(board);
        }
        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxDelete(int id)
        {
            var board = await _context.Board.FindAsync(id);
            if (board == null)
                return NotFound(new { success = false, message = "Board not found." });
            _context.Board.Remove(board);
            await _context.SaveChangesAsync();
            return Json(new { success = true, id });
        }
        **/
    }
}