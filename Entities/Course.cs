using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    // Representa la materia/curso.
    public class Course
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Activity> Activities { get; set; } = new List<Activity>();
    }
}
