using CRUD_Application.Data;
using CRUD_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace CRUD_Application.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        // function to update "completed" attribute in database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateCompletedValue([FromBody] TaskUpdateDto data)
        {
            var task = await _context.Task.FindAsync(data.Id);

            task.Completed = data.Completed;
            await _context.SaveChangesAsync();

            return Ok();
        }
        public class TaskUpdateDto
        {
            public int Id { get; set; }
            public int Completed { get; set; }
        }


        // function to add a new task to the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Tasks/CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto data)
        {
            var newTask = new Models.Task
            {
                Content = data.Content,
                ListId = data.ListId,
                Completed = 0,
                Starred = 0,
                Position = data.Position,
                CreatedAt = data.CreatedAt ?? DateTime.Now.ToString(),
                UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString()
            };

            _context.Task.Add(newTask);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public class CreateTaskDto
        {
            public string Content { get; set; }
            public int ListId { get; set; }
            public int Position { get; set; }
            public string CreatedAt { get; set; }
            public string UpdatedAt { get; set; }
        }


        // function to remove a task from the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Tasks/DeleteTask")]
        public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskDto data)
        {
            var taskStub = new Models.Task { Id = data.TaskId };

            _context.Entry(taskStub).State = EntityState.Deleted;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("Task already deleted or doesn't exist.");
            }
        }
        public class DeleteTaskDto
        {
            public int TaskId { get; set; }
        }
    }
}
