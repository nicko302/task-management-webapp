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

    }
}
