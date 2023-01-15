using UnitSelection.Services.CourseServices.Contract.Dto;

namespace UnitSelection.TestTools.CourseTestTools;

public class AddCourseDtoBuilder
{
    private AddCourseDto _dto;


    public AddCourseDtoBuilder()
    {
        _dto = new AddCourseDto
        {
            Name = "dummy",
            DayOfWeek = "dummy",
            UnitCount = 3,
            StartHour = "10:00",
            EndHour = "13:00",
            ClassId = 1,
            GroupOfCourse = "dummyGroup"
        };
    }
    
    public AddCourseDtoBuilder WithName(string name)
    {
        _dto.Name = name;
        return this;
    }
    
    public AddCourseDtoBuilder WithDayOfWeek(string day)
    {
        _dto.Name = day;
        return this;
    }
    
    public AddCourseDtoBuilder WithUnitCount(int unitCount)
    {
        _dto.UnitCount = unitCount;
        return this;
    }
    
    public AddCourseDtoBuilder WithStartHour(string startHour)
    {
        _dto.StartHour = startHour;
        return this;
    }

    public AddCourseDtoBuilder WithEndHour(string endHour)
    {
        _dto.EndHour = endHour;
        return this;
    }
    
    public AddCourseDtoBuilder WithGroupOFCourse(string course)
    {
        _dto.GroupOfCourse = course;
        return this;
    }
    
    public AddCourseDtoBuilder WithClassId(int classId)
    {
        _dto.ClassId = classId;
        return this;
    }

    public AddCourseDto Build()
    {
        return _dto;
    }
}