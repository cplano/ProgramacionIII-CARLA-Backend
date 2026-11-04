namespace Entities
{
    public class Student : Person
    {
        public string File { get; set; } = string.Empty;

        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
