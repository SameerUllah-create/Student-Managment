using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementSystem.Data;      // For AppDbContext
using StudentManagementSystem.Models;    // For Student and HistoryRecord
using Microsoft.EntityFrameworkCore;     // For Entity Framework Core functionalities

class StudentManagement
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n**** Student Management System ****");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. Update Student");
            Console.WriteLine("3. Display All Students");
            Console.WriteLine("4. Search Student by First Name");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    UpdateStudent();
                    break;
                case "3":
                    DisplayAllStudents();
                    break;
                case "4":
                    SearchStudentByFirstName();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }

    static void AddStudent()
    {
        try
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Enter Last Name: ");
                string lastName = Console.ReadLine();

                var student = new Student { FirstName = firstName, LastName = lastName };
                db.Students.Add(student);
                db.SaveChanges();

                Console.WriteLine("Student added successfully to the database!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: Unable to add student. Details: {ex.Message}");
        }
    }

    static void UpdateStudent()
    {
        try
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter Student ID to update: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid ID! Please enter a numeric value.");
                    return;
                }

                var student = db.Students.Include(s => s.History).FirstOrDefault(s => s.Id == id);

                if (student != null)
                {
                    string oldRecord = $"ID: {student.Id}, Name: {student.FirstName} {student.LastName}";
                    student.History.Add(new HistoryRecord { Record = oldRecord });

                    Console.WriteLine("\n**** Enter New Information ****");

                    Console.Write("Enter New First Name (leave empty to keep unchanged): ");
                    string newFirstName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newFirstName))
                    {
                        student.FirstName = newFirstName;
                    }

                    Console.Write("Enter New Last Name (leave empty to keep unchanged): ");
                    string newLastName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newLastName))
                    {
                        student.LastName = newLastName;
                    }

                    db.SaveChanges();

                    Console.WriteLine("\n**** Record Updated Successfully! ****");
                    Console.WriteLine($"Previous Record: {oldRecord}");
                    Console.WriteLine($"Updated Record: ID: {student.Id}, Name: {student.FirstName} {student.LastName}");
                }
                else
                {
                    Console.WriteLine("Student not found!");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: Unable to update student. Details: {ex.Message}");
        }
    }

    static void DisplayAllStudents()
    {
        try
        {
            using (var db = new AppDbContext())
            {
                var students = db.Students.ToList();

                Console.WriteLine("\n**** All Students ****");
                if (students.Count > 0)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.FirstName} {student.LastName}");
                    }
                }
                else
                {
                    Console.WriteLine("No students to display.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: Unable to display students. Details: {ex.Message}");
        }
    }

    static void SearchStudentByFirstName()
    {
        try
        {
            using (var db = new AppDbContext())
            {
                Console.Write("\nEnter First Name to search: ");
                string firstName = Console.ReadLine().ToLower(); // Convert input to lower case

                var result = db.Students
                    .Where(s => s.FirstName.ToLower() == firstName) // Compare in lower case
                    .ToList();

                if (result.Count > 0)
                {
                    Console.WriteLine("\n**** Search Results ****");
                    foreach (var student in result)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.FirstName} {student.LastName}");
                    }
                }
                else
                {
                    Console.WriteLine("No student found with that first name.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: Unable to search students. Details: {ex.Message}");
        }
    }

}
