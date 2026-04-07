using System.ComponentModel.DataAnnotations;

namespace CRUD_Application.Models
{
    public class User
    {
        [Key] public int Id { get; set; } // PK INT AUTOINCREMENT
        public string Email { get; set; } // TEXT NOT NULL UNIQUE
        public string Username { get; set; } // TEXT NOT NULL UNIQUE
        public string Password { get; set; } // TEXT NOT NULL
    }
}
