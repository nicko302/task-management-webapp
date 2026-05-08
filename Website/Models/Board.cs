using System.ComponentModel.DataAnnotations;

namespace CRUD_Application.Models
{
    public class Board
    {
        [Key] public int Id { get; set; } // PK INT AUTOINCREMENT
        public string Name { get; set; } // TEXT NOT NULL
        public string? Desc { get; set; } // TEXT
        public string CreatedAt { get; set; } // TEXT NOT NULL
        public string UpdatedAt { get; set; } // TEXT NOT NULL
        public string? AdminId { get; set; } // INT
        public int? JoinCode { get; set; } // INT

        public virtual ICollection<Models.List> Lists { get; set; } = new List<Models.List>();
        public virtual ICollection<UserHasBoard> UserHasBoards { get; set; }
    }

}
