using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace CRUD_Application.Models
{
    public class UserHasBoard
    {
        [Key] public int user_board_id { get; set; }

        public string UserId { get; set; } // FK STR
        public int BoardId { get; set; } // FK INT

        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; } // Reference navigation

        [ForeignKey("BoardId")]
        public virtual Board Board { get; set; } // Reference navigation

        public int Position { get; set; } // INT NOT NULL

    }
}
