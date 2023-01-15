﻿namespace UnitSelection.Services.CourseServices.Contract.Dto;

public class GetCourseByIdDto
{
    public string Name { get; set; } = String.Empty;
    public int UnitCount { get; set; }
    public string StartHour { get; set; } = String.Empty;
    public string EndHour { get; set; } = String.Empty;
    public string DayOfWeek { get; set; } = string.Empty;
    public int ClassId { get; set; }
    public string GroupOfCourse { get; set; } = string.Empty;

}