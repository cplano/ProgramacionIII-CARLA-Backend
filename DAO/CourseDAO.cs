using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAO
{
    public class CourseDAO
    {
        public List<Course> GetAll() => MockDatabase.Courses;

        public Course? GetById(long id) => MockDatabase.Courses.FirstOrDefault(c => c.Id == id);

        public Course Create(Course course)
        {
            course.Id = MockDatabase.Courses.Any() ? MockDatabase.Courses.Max(c => c.Id) + 1 : 1;
            MockDatabase.Courses.Add(course);
            return course;
        }

        public bool Update(long id, Course updatedCourse)
        {
            var course = GetById(id);
            if (course == null) return false;

            course.Name = updatedCourse.Name;
            return true;
        }

        public bool Delete(long id)
        {
            var course = GetById(id);
            if (course == null) return false;
            return MockDatabase.Courses.Remove(course);
        }

    }
}
