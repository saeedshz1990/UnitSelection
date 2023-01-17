using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.TestTools.TeacherTestTools;

public class UpdateTeacherDtoBuilder
{
    private readonly UpdateTeacherDto _dto;

    public UpdateTeacherDtoBuilder()
    {
        _dto = new UpdateTeacherDto
        {
            FatherName = "dummyFather",
            LastName = "dummyLast",
            FirstName = "dummyFirst",
            DateOfBirth = "dummyBirth",
            GroupOfCourse = "dummyGroup",
            Address = "dummyAddress",
            Diploma = "dummyBachelor",
            Study = "dummyCom",
            CourseId = 1,
        };
    }

    public UpdateTeacherDtoBuilder WithFirstName(string firstName)
    {
        _dto.FirstName = firstName;
        return this;
    }

    public UpdateTeacherDtoBuilder WithLastName(string lastName)
    {
        _dto.LastName = lastName;
        return this;
    }

    public UpdateTeacherDtoBuilder WithFatherName(string fatherName)
    {
        _dto.FatherName = fatherName;
        return this;
    }
    
    public UpdateTeacherDtoBuilder WithDateOfBirth(string birth)
    {
        _dto.DateOfBirth = birth;
        return this;
    }

    public UpdateTeacherDtoBuilder WithGroupOfCourse(string groupCourse)
    {
        _dto.GroupOfCourse = groupCourse;
        return this;
    }

    public UpdateTeacherDtoBuilder WithAddress(string address)
    {
        _dto.Address = address;
        return this;
    }

    public UpdateTeacherDtoBuilder WithDiploma(string diploma)
    {
        _dto.Diploma = diploma;
        return this;
    }

    public UpdateTeacherDtoBuilder WithStudy(string study)
    {
        _dto.Study = study;
        return this;
    }
    
    public UpdateTeacherDtoBuilder WithCourseId(int courseId)
    {
        _dto.CourseId = courseId;
        return this;
    }

    public UpdateTeacherDto Build()
    {
        return _dto;
    }
}