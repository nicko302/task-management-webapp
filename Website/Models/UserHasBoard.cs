using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace CRUD_Application.Models
{
    public class UserHasBoard
    {
        [Key] public int user_board_id { get; set; }

        public int UserId { get; set; } // FK INT
        public int BoardId { get; set; } // FK INT

        public User User { get; set; } // Reference navigation
        public Board Board { get; set; } // Reference navigation

        public int Position { get; set; } // INT NOT NULL

    }
}
