using UnitSelection.Services.ClassServices.Contract.Dto;
using UnitSelection.Services.StudentServices.Contracts.Dto;
using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.TestTools.StudentTestTools;

public class UpdateStudentDtoBuilder
{
    private UpdateStudentDto _dto;

    public UpdateStudentDtoBuilder()
    {
        _dto = new UpdateStudentDto
        {
            FirstName = "dummyFirst",
            LastName = "dummyLast",
            FatherName = "dummyFather",
            Address = "dummyAdd",
            DateOfBirth = "dummyDate",
            Mobile = new GetMobileDto("98", "dummyMob"),
        };
    }
    
    public UpdateStudentDtoBuilder WithFirstName(string firstName)
    {
        _dto.FirstName = firstName;
        return this;
    }

    public UpdateStudentDtoBuilder WithLastName(string lastName)
    {
        _dto.LastName = lastName;
        return this;
    }

    public UpdateStudentDtoBuilder WithFatherName(string fatherName)
    {
        _dto.FatherName = fatherName;
        return this;
    }

    public UpdateStudentDtoBuilder WithDateOfBirth(string birth)
    {
        _dto.DateOfBirth = birth;
        return this;
    }
    
    public UpdateStudentDtoBuilder WithAddress(string address)
    {
        _dto.Address = address;
        return this;
    }

    public UpdateStudentDtoBuilder WithMobileNumber(string mobile, string code)
    {
        _dto.Mobile = new GetMobileDto(code, mobile);
        return this;
    }
    
    public UpdateStudentDto Build()
    {
        return _dto;
    }
}