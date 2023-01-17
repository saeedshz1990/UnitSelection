using UnitSelection.Entities.Courses;

namespace UnitSelection.TestTools.CourseTestTools;

public class CourseDtoBuilder
{
    private readonly Course _course;

    public CourseDtoBuilder()
    {
        _course = new Course
        {
            Name = "dummy",
            DayOfWeek = "dummy",
            UnitCount = 3,
            StartHour = "10:00",
            EndHour = "13:00",
            ClassId = 1
        };
    }

    public CourseDtoBuilder WithName(string name)
    {
        _course.Name = name;
        return this;
    }
    
    public CourseDtoBuilder WithDayOfWeek(string day)
    {
        _course.Name = day;
        return this;
    }
    
    public CourseDtoBuilder WithUnitCount(int unitCount)
    {
        _course.UnitCount = unitCount;
        return this;
    }
    
    public CourseDtoBuilder WithStartHour(string startHour)
    {
        _course.StartHour = startHour;
        return this;
    }

    public CourseDtoBuilder WithEndHour(string endHour)
    {
        _course.EndHour = endHour;
        return this;
    }
    
    public CourseDtoBuilder WithClassId(int classId)
    {
        _course.ClassId = classId;
        return this;
    }

    public Course Build()
    {
        return _course;
    }
}