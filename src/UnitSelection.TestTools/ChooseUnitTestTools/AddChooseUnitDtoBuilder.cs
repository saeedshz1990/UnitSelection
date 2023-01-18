using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;

namespace UnitSelection.TestTools.ChooseUnitTestTools;

public class AddChooseUnitDtoBuilder
{
    private readonly AddChooseUnitDto _dto;

    public AddChooseUnitDtoBuilder()
    {
        _dto = new AddChooseUnitDto()
        {
            ClassId = 1,
            CourseId = 1,
            StudentId = 1,
            TeacherId = 1,
            TermId = 1
        };
    }
    
    public AddChooseUnitDtoBuilder WithClassId(int classId)
    {
        _dto.ClassId = classId;
        return this;
    }

    public AddChooseUnitDtoBuilder WithCourseId(int courseId)
    {
        _dto.CourseId = courseId;
        return this;
    }

    public AddChooseUnitDtoBuilder WithStudentId(int studentId)
    {
        _dto.StudentId = studentId;
        return this;
    }

    public AddChooseUnitDtoBuilder WithTeacherId(int teacherId)
    {
        _dto.TeacherId = teacherId;
        return this;
    }

    public AddChooseUnitDtoBuilder WithTermId(int termId)
    {
        _dto.TermId = termId;
        return this;
    }

    public AddChooseUnitDto Build()
    {
        return _dto;
    }
}