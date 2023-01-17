using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.TestTools.TeacherTestTools;

public class AddTeacherDtoBuilder
{
    private readonly AddTeacherDto _dto;

    public AddTeacherDtoBuilder()
    {
        _dto = new AddTeacherDto
        {
            FatherName = "dummyFather",
            LastName = "dummyLast",
            FirstName = "dummyFirst",
            NationalCode = "dummyCode",
            DateOfBirth = "dummyBirth",
            GroupOfCourse = "dummyGroup",
            Address = "dummyAddress",
            Diploma = "dummyBachelor",
            Study = "dummyCom",
            Mobile = new 
                GetMobileDto(
                    "98",
                    "dummyMob"),
            CourseId = 1,
        };
    }

    public AddTeacherDtoBuilder WithFirstName(string firstName)
    {
        _dto.FirstName = firstName;
        return this;
    }

    public AddTeacherDtoBuilder WithLastName(string lastName)
    {
        _dto.LastName = lastName;
        return this;
    }

    public AddTeacherDtoBuilder WithFatherName(string fatherName)
    {
        _dto.FatherName = fatherName;
        return this;
    }

    public AddTeacherDtoBuilder WithNationalCode(string code)
    {
        _dto.NationalCode = code;
        return this;
    }

    public AddTeacherDtoBuilder WithDateOfBirth(string birth)
    {
        _dto.DateOfBirth = birth;
        return this;
    }

    public AddTeacherDtoBuilder WithGroupOfCourse(string groupCourse)
    {
        _dto.GroupOfCourse = groupCourse;
        return this;
    }

    public AddTeacherDtoBuilder WithAddress(string address)
    {
        _dto.Address = address;
        return this;
    }

    public AddTeacherDtoBuilder WithDiploma(string diploma)
    {
        _dto.Diploma = diploma;
        return this;
    }

    public AddTeacherDtoBuilder WithStudy(string study)
    {
        _dto.Study = study;
        return this;
    }

    public AddTeacherDtoBuilder WithMobileNumber(string mobile, string code)
    {
        _dto.Mobile = new GetMobileDto(code, mobile);
        return this;
    }

    public AddTeacherDtoBuilder WithCourseId(int courseId)
    {
        _dto.CourseId = courseId;
        return this;
    }

    public AddTeacherDto Build()
    {
        return _dto;
    }
}