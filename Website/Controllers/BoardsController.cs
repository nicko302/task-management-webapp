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

        // display this page if the user has no boards
        public IActionResult NoBoards()
        {
            return View();
        }


        // display the page with the associated board id
        [HttpGet("/Boards/Index/{id?}")]
        public async Task<IActionResult> Index(int? id)
        {
            var userId = 1; ///////// REMEMBER TO CHANGE WHEN LOGIN WORKS

            // return the data from the first board belonging to the user
            var firstBoard = await _context.UserHasBoard
                .Where(uhb => uhb.UserId == userId)
                .Include(uhb => uhb.Board.Lists.OrderBy(l => l.Position))
                    .ThenInclude(l => l.Tasks.OrderBy(t => t.Position))
                .FirstOrDefaultAsync(m => m.Position == 1);

            // return all board IDs of boards belonging to the current user
            var boardsOfUser = await _context.UserHasBoard
               .Where(uhb => uhb.UserId == userId)
               .OrderBy(uhb => uhb.Position)
               .Select(uhb => uhb.BoardId)
               .ToListAsync();


            // if the user has NO boards
            if (boardsOfUser.Count == 0)
            {
                return RedirectToAction("NoBoards");
            }

            // if the entered id is null, 0, or not their board
            else if (!id.HasValue || id == 0 || !boardsOfUser.Contains((int)id))
            {
                //redirect to the user's first board
                return RedirectToAction("Index", new { id = boardsOfUser[0] });
                //return View("Index", firstBoard);
            }

            
            // return all needed board data of the given board ID
            var board = await _context.Board
                .Include(b => b.Lists.OrderBy(l => l.Position))
                    .ThenInclude(l => l.Tasks.OrderBy(t => t.Position))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (board == null)
            {
                return RedirectToAction("NoBoards");
            }

            // if a valid ID is present in the URL, successfully display the board
            return View("Index", board);
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

        // function to edit an existing board's name AND description in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/EditBoard")]
        public async Task<IActionResult> EditBoard([FromBody] EditBoardDto data)
        {
            var existingBoard = await _context.Board.FindAsync(data.BoardId);

            if (existingBoard == null)
            {
                return NotFound($"Board with ID {data.BoardId} not found.");
            }

            existingBoard.Name = data.NewName;
            existingBoard.Desc = data.NewDesc;
            existingBoard.JoinCode = data.JoinCode;
            existingBoard.UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString();

            await _context.SaveChangesAsync();

            return Ok();
        }
        public class EditBoardDto
        {
            public int BoardId { get; set; }
            public string NewName { get; set; }
            public string NewDesc { get; set; }
            public int JoinCode { get; set; }
            public string UpdatedAt { get; set; }
        }



        // function to remove a board from the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/DeleteBoard")]
        public async Task<IActionResult> DeleteBoard([FromBody] DeleteBoardDto data)
        {
            var userId = 1; /////////////// replace with actual user ID retrieval logic

            // fetch the board (Board and UserHasBoard entry) to be deleted
            var deletedBoard = await _context.UserHasBoard
               .Include(u => u.Board)
               .Where(u => u.BoardId == data.BoardId)
               .FirstOrDefaultAsync();

            // retrieve the deleted board's position value
            int deletedBoardPosition = deletedBoard.Position;

            // fetch boards belonging to the current user, positioned AFTER the deleted board
            var proceedingBoards = await _context.UserHasBoard
               .Include(u => u.Board)
               .Where(u => u.UserId == userId && u.Position > deletedBoardPosition)
               .ToListAsync();

            // lower the position value of all proceeding boards by 1
            foreach (var board in proceedingBoards)
            {
                board.Position -= 1;
            }

            // delete the board and save new position values
            _context.Board.Remove(deletedBoard.Board);
            await _context.SaveChangesAsync();

            // retrieve the board ID of the user's top board, to redirect to after deletion
            var firstBoardId = await _context.UserHasBoard
               .Where(u => u.UserId == userId && u.Position == 1)
               .Select(u => u.BoardId)
               .FirstOrDefaultAsync();

            return Ok(firstBoardId);
        }
        public class DeleteBoardDto
        {
            public int BoardId { get; set; }
        }



        // function to change a board's position value in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/MoveBoard")]
        public async Task<IActionResult> MoveBoard([FromBody] MoveBoardDto data)
        {
            var userId = 1; /////////////// replace with actual user ID retrieval logic


            // find the board to be moved
            var movedBoard = await _context.UserHasBoard
               .Include(u => u.Board)
               .Where(u => u.BoardId == data.BoardId && u.UserId == userId)
               .FirstOrDefaultAsync();
            if (movedBoard == null) return NotFound();

            bool isShared = movedBoard.Board.AdminId != null;

            int movedBoardPosition = movedBoard.Position;
            int boardId = movedBoard.BoardId;


            // fetch boards (either private or shared) it can be swapped with
            var allBoards = await _context.UserHasBoard
               .Where(u => u.UserId == userId && (isShared ? u.Board.AdminId != null : u.Board.AdminId == null))
               .ToListAsync();


            if (data.Direction == "up")
            {
                foreach (var board in allBoards)
                {
                    if (board.Position == movedBoardPosition - 1)
                    {
                        // effectively swap the position values of the current and the previous list
                        board.Position += 1;
                        movedBoard.Position -= 1;
                        break;
                    }
                }
            }
            else if (data.Direction == "down")
            {
                foreach (var board in allBoards)
                {
                    if (board.Position == movedBoardPosition + 1)
                    {
                        // effectively swap the position values of the current and the following list
                        board.Position -= 1;
                        movedBoard.Position += 1;
                        break;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok();

        }
        public class MoveBoardDto
        {
            public int BoardId { get; set; }
            public string Direction { get; set; }
        }


        // function to get a shared board's join code from the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/GetJoinCode")]
        public async Task<IActionResult> GetJoinCode([FromBody] GetJoinCodeDto data)
        {
            var board = await _context.Board.FindAsync(data.BoardId);
            return Ok(new { code = board.JoinCode });
        }
        public class GetJoinCodeDto
        {
            public int BoardId { get; set; }
        }


        // function to get a shared board's join code from the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/GetBoardMembers")]
        public async Task<IActionResult> GetBoardMembers([FromBody] GetBoardMembersDto data)
        {
            var board = await _context.Board.FindAsync(data.BoardId);

            var memberNames = await _context.UserHasBoard
               .Where(u => u.BoardId == data.BoardId)
               .Select(u => u.User.Username)
               .ToListAsync();

            var groupAdminID = await _context.UserHasBoard
               .Where(u => u.BoardId == data.BoardId)
               .Select(u => u.Board.AdminId)
               .FirstOrDefaultAsync();
            var groupAdminName = await _context.UserHasBoard
               .Where(u => u.UserId == groupAdminID)
               .Select(u => u.User.Username)
               .FirstOrDefaultAsync();

            return Ok( new { members = memberNames, admin = groupAdminName });
        }
        public class GetBoardMembersDto
        {
            public int BoardId { get; set; }
        }


        // function to convert an existing board to a shared board
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/ConvertBoard")]
        public async Task<IActionResult> ConvertBoard([FromBody] ConvertBoardDto data)
        {
            var userId = 1; ////////////// replace when user login system works

            
            // retrieve the user board connection
            var movedBoard = await _context.UserHasBoard
                .Include(uhb => uhb.Board)
                .FirstOrDefaultAsync(uhb => uhb.BoardId == data.BoardId && uhb.UserId == userId);
            if (movedBoard == null) // check the board exists
            {
                return NotFound($"Board {data.BoardId} not found.");
            }

            // calculate the new position of the moved board
            var boardsOfUser = await _context.UserHasBoard
               .Where(uhb => uhb.UserId == userId && uhb.Board.AdminId != null)
               .ToListAsync();
            var newBoardPosition = boardsOfUser.Count() + 1;

            movedBoard.Position = newBoardPosition;
            movedBoard.Board.AdminId = userId;
            movedBoard.Board.JoinCode = data.JoinCode;
            movedBoard.Board.UpdatedAt = data.UpdatedAt ?? DateTime.Now.ToString();

            await _context.SaveChangesAsync();

            return Ok();
        }
        public class ConvertBoardDto
        {
            public int BoardId { get; set; }
            public int JoinCode { get; set; }
            public string UpdatedAt { get; set; }
        }

        // function to remove a user from a shared board
        [HttpPost]
        [Route("Boards/RemoveMember")]
        public async Task<IActionResult> RemoveMember([FromBody] RemoveMemberDTO data)
        {
            // retrieve the user and board connection
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == data.MemberName);
            var userHasBoard = await _context.UserHasBoard
                .FirstOrDefaultAsync(uhb => uhb.UserId == user.Id && uhb.BoardId == data.BoardId);
            if (userHasBoard == null) { return NotFound("Member is not on this board");  }

            // remove the user from the board
            _context.UserHasBoard.Remove(userHasBoard);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public class RemoveMemberDTO
        {
            public int BoardId { get; set; }
            public string MemberName { get; set; }
        }


        // function to add a user to an existing shared board in the database
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("Boards/UserJoinBoard")]
        public async Task<IActionResult> UserJoinBoard([FromBody] UserJoinBoardDto data)
        {
            var userId = 1; /////////////// REPLACE WHEN LOGIN SYSTEM WORKS

            // check if a board exists with that code
            var boardWithCode = await _context.Board
                .FirstOrDefaultAsync(b => b.JoinCode == data.JoinCode);
            if (boardWithCode == null)
            {
                return NotFound("A board does not exist with that code.");
            }
            var boardId = boardWithCode.Id;

            // check if the user is already a member
            var alreadyMember = await _context.UserHasBoard
               .FirstOrDefaultAsync(uhb => uhb.UserId == userId && uhb.BoardId == boardId);
            if (alreadyMember != null) { return NotFound("Member is not on this board"); }


            // retrieve the position value of the shared board for the new user
            var boardsOfUser = await _context.UserHasBoard
               .Where(uhb => uhb.UserId == userId && uhb.Board.AdminId != null)
               .Select(uhb => uhb.BoardId)
               .ToListAsync();
            var newBoardPosition = boardsOfUser.Count() + 1;


            // create and save the new UserHasBoard entry
            var newUserHasBoard = new Models.UserHasBoard
            {
                UserId = userId,
                BoardId = boardId,
                Position = newBoardPosition
            };

            _context.UserHasBoard.Add(newUserHasBoard);
            await _context.SaveChangesAsync();

            return Ok(boardId);
        }
        public class UserJoinBoardDto
        {
            public int JoinCode { get; set; }
        }
    }
}