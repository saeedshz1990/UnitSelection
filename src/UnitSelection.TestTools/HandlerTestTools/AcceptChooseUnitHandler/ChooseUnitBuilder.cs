using UnitSelection.Entities.ChooseUnits;

namespace UnitSelection.TestTools.HandlerTestTools.AcceptChooseUnitHandler;

public class ChooseUnitBuilder
{
    private ChooseUnit _dto;

    public ChooseUnitBuilder()
    {
        _dto = new ChooseUnit
        {
            ClassId = 1,
            CourseId = 1,
            StudentId = 1,
            TeacherId = 1,
            TermId = 1
        };
    }
    
    public ChooseUnitBuilder WithClassId(int classId)
    {
        _dto.ClassId = classId;
        return this;
    }

    public ChooseUnitBuilder WithCourseId(int courseId)
    {
        _dto.CourseId = courseId;
        return this;
    }

    public ChooseUnitBuilder WithStudentId(int studentId)
    {
        _dto.StudentId = studentId;
        return this;
    }

    public ChooseUnitBuilder WithTeacherId(int teacherId)
    {
        _dto.TeacherId = teacherId;
        return this;
    }

    public ChooseUnitBuilder WithTermId(int termId)
    {
        _dto.TermId = termId;
        return this;
    }

    public ChooseUnit Build()
    {
        return _dto;
    }
}