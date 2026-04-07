using System.ComponentModel.DataAnnotations;

namespace CRUD_Application.Models
{
    public class List
    {
        [Key] public int Id { get; set; } // PK INT AUTOINCREMENT

        public int BoardId { get; set; } // FK INT NOT NULL
        public string Name { get; set; } // TEXT NOT NULL
        public string Colour { get; set; } // TEXT NOT NULL
        public int Position { get; set; } // INT NOT NULL
        public string CreatedAt { get; set; } // TEXT NOT NULL
        public string UpdatedAt { get; set; } // TEXT NOT NULL

        public Board Board { get; set; } // Reference navigation
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>(); // Reference navigation
    }
}