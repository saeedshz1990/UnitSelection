using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Teachers;

namespace UnitSelection.Entities.Courses;

public class Course
{
    public Course()
    {
        Teachers = new HashSet<Teacher>();
    }
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UnitCount { get; set; }
    public string StartHour { get; set; }= string.Empty;
    public string EndHour { get; set; }= string.Empty;
    public string DayOfWeek { get; set; } = string.Empty;
    public string GroupOfCourse { get; set; } = string.Empty;
    public int ClassId { get; set; }
    public Class Class { get; set; }

    public HashSet<Teacher> Teachers { get; set; }
}