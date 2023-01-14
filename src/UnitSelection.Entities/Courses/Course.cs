using UnitSelection.Entities.Classes;

namespace UnitSelection.Entities.Courses;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UnitCount { get; set; }
    public string StartHour { get; set; }= string.Empty;
    public string EndHour { get; set; }= string.Empty;
    public string DayOfWeek { get; set; } = string.Empty;
    public int ClassId { get; set; }
    public Class Class { get; set; }
}