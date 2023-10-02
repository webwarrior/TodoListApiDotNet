using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.Models.dao
{
    public class TodoItem
    {
        [Key]  // Indicates this property is the primary key in the database table
        public Guid Id { get; set; }

        [Required]  // Indicates this property is required and cannot be null in the database table
        [MaxLength(500)]  // Indicates a maximum length constraint for the string value
        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Updated { get; set; } = DateTime.Now;
    }
}
