using CRUD_Application.Data;
using CRUD_Application.Models;
using Microsoft.AspNetCore.Hosting.Server;
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


        // function to add a new board to the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/CreateBoard")]
        public async Task<IActionResult> CreateBoard([FromBody] CreateBoardDto data)
        {
            // create and save the new board
            var newBoard = new Models.Board
            {
                Name = data.Name,
                Desc = "No description",
                CreatedAt = data.CreatedAt ?? DateTime.Now.ToString(),
                UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString(),
            };

            _context.Board.Add(newBoard);
            await _context.SaveChangesAsync();

            int userId = 1; /////////////// MAKE SURE TO CHANGE THIS TO USE ACTUAL USERID

            // create the join table entry
            var newUserHasBoard = new Models.UserHasBoard
            {
                UserId = userId,
                BoardId = newBoard.Id,
                Position = data.Position
            };

            _context.UserHasBoard.Add(newUserHasBoard);
            await _context.SaveChangesAsync();

            return Ok(newBoard.Id);
        }
        public class CreateBoardDto
        {
            public string Name { get; set; }
            public int Position { get; set; }
            public string CreatedAt { get; set; }
            public string UpdatedAt { get; set; }
        }

        // function to edit an existing board's NAME in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/EditBoardName")]
        public async Task<IActionResult> EditBoardName([FromBody] EditBoardNameDto data)
        {
            var existingBoard = await _context.Board.FindAsync(data.BoardId);

            if (existingBoard == null)
            {
                return NotFound($"Board with ID {data.BoardId} not found.");
            }

            existingBoard.Name = data.NewName;
            existingBoard.UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString();

            await _context.SaveChangesAsync();

            return Ok();
        }
        public class EditBoardNameDto
        {
            public int BoardId { get; set; }
            public string NewName { get; set; }
            public string UpdatedAt { get; set; }
        }

        // function to edit an existing board's DESCRIPTION in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/EditBoardDesc")]
        public async Task<IActionResult> EditBoardDesc([FromBody] EditBoardDescDto data)
        {
            var existingBoard = await _context.Board.FindAsync(data.BoardId);

            if (existingBoard == null)
            {
                return NotFound($"Board with ID {data.BoardId} not found.");
            }

            existingBoard.Desc = data.NewDesc;
            existingBoard.UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString();

            await _context.SaveChangesAsync();

            return Ok();
        }
        public class EditBoardDescDto
        {
            public int BoardId { get; set; }
            public string NewDesc { get; set; }
            public string UpdatedAt { get; set; }
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