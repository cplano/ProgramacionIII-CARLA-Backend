using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAO
{
    public class StudentDAO
    {
        public List<Student> GetAll() => MockDatabase.Students;

        public Student? GetById(long id) => MockDatabase.Students.FirstOrDefault(s => s.Id == id);

        public Student Create(Student student)
        {
            student.Id = MockDatabase.Students.Any() ? MockDatabase.Students.Max(s => s.Id) + 1 : 1;
            MockDatabase.Students.Add(student);
            return student;
        }

        public bool Update(long id, Student updatedStudent)
        {
            var student = GetById(id);
            if (student == null) return false;

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Dni = updatedStudent.Dni;
            student.File = updatedStudent.File;
            return true;
        }

        public bool Delete(long id)
        {
            var student = GetById(id);
            if (student == null) return false;
            return MockDatabase.Students.Remove(student);
        }

    }
}
