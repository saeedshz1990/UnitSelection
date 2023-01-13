namespace UnitSelection.Services.Courses.Contract.Dto;

public class AddCourseDto
{
    public string Name { get; set; } = String.Empty;
    public int UnitCount { get; set; }
    public string StartHour { get; set; }
    public string EndHour { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public int ClassId { get; set; }
}