using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Application.Models
{
    public class Task
    {
        [Key] public int Id { get; set; } // PK INT AUTOINCREMENT

        public int ListId { get; set; } // FK INT NOT NULL
        [ValidateNever]
        public List? List { get; set; } // Reference navigation

        public string Content { get; set; } // TEXT NOT NULL
        public int Completed { get; set; } // INT NOT NULL
        public int Starred { get; set; } // INT NOT NULL
        public int Position { get; set; } // INT NOT NULL
        public string? CreatedAt { get; set; } // TEXT
        public string? UpdatedAt { get; set; } // TEXT
    }
}
