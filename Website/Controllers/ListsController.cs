using CRUD_Application.Data;
using CRUD_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace CRUD_Application.Controllers
{
    public class ListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        // function to add a new list to the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Lists/CreateList")]
        public async Task<IActionResult> CreateList([FromBody] CreateListDto data)
        {
            var newList = new Models.List
            {
                Name = data.Name,
                BoardId = data.BoardId,
                Position = data.Position,
                CreatedAt = data.CreatedAt ?? DateTime.Now.ToString(),
                UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString(),
                Colour = data.Colour
            };

            _context.List.Add(newList);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public class CreateListDto
        {
            public string Name { get; set; }
            public int BoardId { get; set; }
            public int Position { get; set; }
            public string CreatedAt { get; set; }
            public string UpdatedAt { get; set; }
            public string Colour { get; set; }
        }


        // function to edit an existing list in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Lists/EditList")]
        public async Task<IActionResult> EditList([FromBody] EditListDto data)
        {
            var existingList = await _context.List.FindAsync(data.ListId);

            if (existingList == null)
            {
                return NotFound($"List with ID {data.ListId} not found.");
            }

            existingList.Name = data.NewName;
            existingList.UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString();

            await _context.SaveChangesAsync();

            return Ok();
        }
        public class EditListDto
        {
            public int ListId { get; set; }
            public string NewName { get; set; }
            public string UpdatedAt { get; set; }
        }
        // function to edit a list's colour in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Lists/EditListColour")]
        public async Task<IActionResult> EditListColour([FromBody] EditListColourDto data)
        {
            var existingList = await _context.List.FindAsync(data.ListId);

            if (existingList == null)
            {
                return NotFound($"List with ID {data.ListId} not found.");
            }

            existingList.Colour = data.NewColour;
            existingList.UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString();

            await _context.SaveChangesAsync();

            return Ok();
        }
        public class EditListColourDto
        {
            public int ListId { get; set; }
            public string NewColour { get; set; }
            public string UpdatedAt { get; set; }
        }


        // function to remove a list from the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Lists/DeleteList")]
        public async Task<IActionResult> DeleteList([FromBody] DeleteListDto data)
        {
            var deletedList = await _context.List.FindAsync(data.ListId);
            int deletedListPosition = deletedList.Position;
            int boardId = deletedList.BoardId;

            List<Models.List> allListsInBoard = await _context.List.Where(l => l.BoardId == boardId).ToListAsync();

            // lower the position value of all proceeding tasks by 1
            foreach (var list in allListsInBoard)
            {
                if (list.Position > deletedListPosition)
                {
                    list.Position -= 1;
                }
            }

            // delete the task and save new position values
            _context.List.Remove(deletedList);
            await _context.SaveChangesAsync();
            return Ok();
        }
        public class DeleteListDto
        {
            public int ListId { get; set; }
        }


        // function to change a list's position value in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Lists/MoveList")]
        public async Task<IActionResult> MoveTask([FromBody] MoveListDto data)
        {
            var movedList = await _context.List.FindAsync(data.ListId); // find the board to be moved
            int movedListPosition = movedList.Position;
            int boardId = movedList.BoardId;

            List<Models.List> allListsInBoard = await _context.List.Where(l => l.BoardId == boardId).ToListAsync(); // retrieve all lists in the board

            if (data.Direction == "left")
            {
                foreach (var list in allListsInBoard)
                {
                    if (list.Position == movedListPosition - 1)
                    {
                        // effectively swap the position values of the current and the previous list
                        list.Position += 1;
                        movedList.Position -= 1;
                    }
                }
            }
            else if (data.Direction == "right")
            {
                foreach (var list in allListsInBoard)
                {
                    if (list.Position == movedListPosition + 1)
                    {
                        // effectively swap the position values of the current and the following list
                        list.Position -= 1;
                        movedList.Position += 1;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok();

        }
        public class MoveListDto
        {
            public int ListId { get; set; }
            public string Direction { get; set; }
        }
    }
}
