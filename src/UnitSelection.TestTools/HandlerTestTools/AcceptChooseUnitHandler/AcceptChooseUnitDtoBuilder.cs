using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts.Dto;

namespace UnitSelection.TestTools.HandlerTestTools.AcceptChooseUnitHandler;

public class AcceptChooseUnitDtoBuilder
{
    private readonly AcceptChooseUnitDto _dto;

    public AcceptChooseUnitDtoBuilder()
    {
        _dto = new AcceptChooseUnitDto
        {
            CourseId = 1,
            StudentId = 1,
            TeacherId = 1,
        };
    }
    
    public AcceptChooseUnitDtoBuilder WithCourseId(int courseId)
    {
        _dto.CourseId = courseId;
        return this;
    }
    
    public AcceptChooseUnitDtoBuilder WithStudentId(int studentId)
    {
        _dto.StudentId = studentId;
        return this;
    }
    
    public AcceptChooseUnitDtoBuilder WithTeacherId(int teacherId)
    {
        _dto.TeacherId = teacherId;
        return this;
    }
    
    public AcceptChooseUnitDto Build()
    {
        return _dto;
    }
}