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
            var deletedTask = await _context.Task.FindAsync(data.TaskId);
            int deletedTaskPosition = deletedTask.Position;
            int listId = deletedTask.ListId;

            List<Models.Task> allTasksInList = await _context.Task.Where(t => t.ListId == listId).ToListAsync();

            // lower the position value of all proceeding tasks by 1
            foreach (var task in allTasksInList)
            {
                if (task.Position > deletedTaskPosition)
                {
                    task.Position -= 1;
                }
            }

            // delete the task and save new position values
            _context.Task.Remove(deletedTask);
            await _context.SaveChangesAsync();
            return Ok();
        }
        public class DeleteTaskDto
        {
            public int TaskId { get; set; }
        }


        // function to edit an existing task in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Tasks/EditTask")]
        public async Task<IActionResult> EditTask([FromBody] EditTaskDto data)
        {
            var existingTask = await _context.Task.FindAsync(data.TaskId);

            if (existingTask == null)
            {
                return NotFound($"Task with ID {data.TaskId} not found.");
            }

            existingTask.Content = data.Content;
            existingTask.UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString();

            await _context.SaveChangesAsync();

            return Ok();
        }
        public class EditTaskDto
        {
            public int TaskId { get; set; }
            public string Content { get; set; }
            public string UpdatedAt { get; set; }
        }


        // function to change a task's position value in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Tasks/MoveTask")]
        public async Task<IActionResult> MoveTask([FromBody] MoveTaskDto data)
        {
            var movedTask = await _context.Task.FindAsync(data.TaskId); // find the task to be moved
            int movedTaskPosition = movedTask.Position;
            int listId = movedTask.ListId;

            List<Models.Task> allTasksInList = await _context.Task.Where(t => t.ListId == listId).ToListAsync(); // retrieve all tasks in the list

            if (data.Direction == "up")
            {
                foreach (var task in allTasksInList)
                {
                    if (task.Position == movedTaskPosition - 1)
                    {
                        // effectively swap the position values of the current and the previous element
                        task.Position += 1;
                        movedTask.Position -= 1;
                    }
                }
            }
            else if (data.Direction == "down")
            {
                foreach (var task in allTasksInList)
                {
                    if (task.Position == movedTaskPosition + 1)
                    {
                        // effectively swap the position values of the current and the following element
                        task.Position -= 1;
                        movedTask.Position += 1;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok();

        }
        public class MoveTaskDto
        {
            public int TaskId { get; set; }
            public string Direction { get; set; }
        }
    }
}
