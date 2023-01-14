namespace UnitSelection.Services.Courses.Contract.Dto;

public class AddCourseDto
{
    public string Name { get; set; } = string.Empty;
    public int UnitCount { get; set; }
    public string StartHour { get; set; } = string.Empty;
    public string EndHour { get; set; } = string.Empty;
    public string DayOfWeek { get; set; } = string.Empty;
    public int ClassId { get; set; }
}