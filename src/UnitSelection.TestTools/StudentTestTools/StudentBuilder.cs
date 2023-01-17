using UnitSelection.Entities;
using UnitSelection.Entities.Students;

namespace UnitSelection.TestTools.StudentTestTools;

public class StudentBuilder
{
    private readonly Student _student;

    public StudentBuilder()
    {
        _student = new Student
        {
            FirstName = "dummyFirst",
            LastName = "dummyLast",
            FatherName = "dummyFather",
            Address = "dummyAdd",
            NationalCode = "dummyNat",
            DateOfBirth = "dummyDate",
            Mobile = new 
                Mobile(
                    "98",
                    "dummyMob"),
        };
    }
    
    public StudentBuilder WithFirstName(string firstName)
    {
        _student.FirstName = firstName;
        return this;
    }

    public StudentBuilder WithLastName(string lastName)
    {
        _student.LastName = lastName;
        return this;
    }

    public StudentBuilder WithFatherName(string fatherName)
    {
        _student.FatherName = fatherName;
        return this;
    }

    public StudentBuilder WithNationalCode(string code)
    {
        _student.NationalCode = code;
        return this;
    }

    public StudentBuilder WithDateOfBirth(string birth)
    {
        _student.DateOfBirth = birth;
        return this;
    }
    
    public StudentBuilder WithAddress(string address)
    {
        _student.Address = address;
        return this;
    }

    public StudentBuilder WithMobileNumber(string mobile, string code)
    {
        _student.Mobile = new Mobile(code, mobile);
        return this;
    }
    
    public Student Build()
    {
        return _student;
    }
}