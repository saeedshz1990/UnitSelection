using UnitSelection.Services.Courses.Contract.Dto;

namespace UnitSelection.TestTools.CourseTestTools;

public class UpdateCourseDtoBuilder
{
    private UpdateCourseDto _dto;

    public UpdateCourseDtoBuilder()
    {
        _dto = new UpdateCourseDto
        {
            Name = "dummy",
            DayOfWeek = "dummy",
            UnitCount = 3,
            StartHour = "10:00",
            EndHour = "13:00",
            ClassId = 1,
            GroupOfCourse = "dummy"
        };
    }
    
    public UpdateCourseDtoBuilder WithName(string name)
    {
        _dto.Name = name;
        return this;
    }
    
    public UpdateCourseDtoBuilder WithDayOfWeek(string day)
    {
        _dto.Name = day;
        return this;
    }
    
    public UpdateCourseDtoBuilder WithUnitCount(int unitCount)
    {
        _dto.UnitCount = unitCount;
        return this;
    }
    
    public UpdateCourseDtoBuilder WithStartHour(string startHour)
    {
        _dto.StartHour = startHour;
        return this;
    }

    public UpdateCourseDtoBuilder WithEndHour(string endHour)
    {
        _dto.EndHour = endHour;
        return this;
    }
    
    public UpdateCourseDtoBuilder WithGroupOFCourse(string course)
    {
        _dto.GroupOfCourse = course;
        return this;
    }
    
    public UpdateCourseDtoBuilder WithClassId(int classId)
    {
        _dto.ClassId = classId;
        return this;
    }

    public UpdateCourseDto Build()
    {
        return _dto;
    }
}