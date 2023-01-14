using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Terms;

namespace UnitSelection.Entities.Classes
{
    public class Class
    {
        public Class()
        {
            Courses = new HashSet<Course>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int TermId { get; set; }
        public Term Term { get; set; }
        
        public HashSet<Course> Courses { get; set; }
    }
}