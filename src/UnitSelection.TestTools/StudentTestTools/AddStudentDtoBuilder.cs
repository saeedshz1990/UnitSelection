using UnitSelection.Services.StudentServices.Contracts.Dto;
using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.TestTools.StudentTestTools;

public class AddStudentDtoBuilder
{
    private AddStudentDto _dto;

    public AddStudentDtoBuilder()
    {
        _dto = new AddStudentDto
        {
            FirstName = "dummyFirst",
            LastName = "dummyLast",
            FatherName = "dummyFather",
            Address = "dummyAdd",
            NationalCode = "dummyNat",
            DateOfBirth = "dummyDate",
            Mobile = new GetMobileDto("98", "dummyMob"),
        };
    }
    
    public AddStudentDtoBuilder WithFirstName(string firstName)
    {
        _dto.FirstName = firstName;
        return this;
    }

    public AddStudentDtoBuilder WithLastName(string lastName)
    {
        _dto.LastName = lastName;
        return this;
    }

    public AddStudentDtoBuilder WithFatherName(string fatherName)
    {
        _dto.FatherName = fatherName;
        return this;
    }

    public AddStudentDtoBuilder WithNationalCode(string code)
    {
        _dto.NationalCode = code;
        return this;
    }

    public AddStudentDtoBuilder WithDateOfBirth(string birth)
    {
        _dto.DateOfBirth = birth;
        return this;
    }
    
    public AddStudentDtoBuilder WithAddress(string address)
    {
        _dto.Address = address;
        return this;
    }

    public AddStudentDtoBuilder WithMobileNumber(string mobile, string code)
    {
        _dto.Mobile = new GetMobileDto(code, mobile);
        return this;
    }
    
    public AddStudentDto Build()
    {
        return _dto;
    }
}