namespace week9
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    namespace ConsoleAppEF
    {
     
        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public double AvgPoint { get; set; }
        }

       
        public class AppDbContext : DbContext
        {
            public DbSet<Student> Students { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StudentDb;Trusted_Connection=True;");
            }
        }

       
        public class StudentService
        {
            private static readonly AppDbContext _context = new AppDbContext();

            public List<Student> GetAll() => _context.Students.ToList();

            public Student GetById(int id) => _context.Students.FirstOrDefault(s => s.Id == id);

            public void Create(Student student)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
            }

            public void Delete(int id)
            {
                var studentToDelete = GetById(id);

                if (studentToDelete != null)
                {
                    _context.Students.Remove(studentToDelete);
                    _context.SaveChanges();
                }
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                using (var context = new AppDbContext())
                {
                 
                    context.Database.EnsureCreated();
                }

                
                var studentService = new StudentService();

           
                studentService.Create(new Student { Name = "Nihat", Surname = "rehimzade", AvgPoint = 85.5 });

                
                var allStudents = studentService.GetAll();
                Console.WriteLine("All Students:");
                allStudents.ForEach(student =>
                    Console.WriteLine($"Id: {student.Id}, Name: {student.Name}, Surname: {student.Surname}, AvgPoint: {student.AvgPoint}")
                );

               
                var studentById = studentService.GetById(1);
                Console.WriteLine(studentById != null
                    ? $"Student with Id 1: {studentById.Name} {studentById.Surname}"
                    : "Student not found."
                );

              
                studentService.Delete(1);

                Console.ReadLine();
            }
        }
    }

}