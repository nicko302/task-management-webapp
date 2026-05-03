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

        public string HexColour => Colour?.ToLower() switch
        {
            "red" => "#451b12",
            "green" => "#103821",
            "blue" => "#0d365c",
            "orange" => "#a1620b",
            "purple" => "#4d2d47",
            "teal" => "#1d524b",
            "brown" => "#3d332a",
            "yellow" => "#bdad39",
            _ => "#333333"
        };
    }
}