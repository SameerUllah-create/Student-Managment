using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        [Key]  // Primary Key
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        // Stores the change history as strings
        public List<HistoryRecord> History { get; set; } = new List<HistoryRecord>();
    }

    public class HistoryRecord
    {
        public int Id { get; set; }
        public string Record { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.Now;
    }
}
