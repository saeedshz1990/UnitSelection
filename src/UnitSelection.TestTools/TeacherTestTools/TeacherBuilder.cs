using UnitSelection.Entities;
using UnitSelection.Entities.Teachers;

namespace UnitSelection.TestTools.TeacherTestTools;

public class TeacherBuilder
{
    private Teacher _dto;

    public TeacherBuilder()
    {
        _dto = new Teacher
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
            Mobile = new Mobile("98", "dummyMob"),
            CourseId = 1,
        };
    }

    public TeacherBuilder WithFirstName(string firstName)
    {
        _dto.FirstName = firstName;
        return this;
    }

    public TeacherBuilder WithLastName(string lastName)
    {
        _dto.LastName = lastName;
        return this;
    }

    public TeacherBuilder WithFatherName(string fatherName)
    {
        _dto.FatherName = fatherName;
        return this;
    }

    public TeacherBuilder WithNationalCode(string code)
    {
        _dto.NationalCode = code;
        return this;
    }

    public TeacherBuilder WithDateOfBirth(string birth)
    {
        _dto.DateOfBirth = birth;
        return this;
    }

    public TeacherBuilder WithGroupOfCourse(string groupCourse)
    {
        _dto.GroupOfCourse = groupCourse;
        return this;
    }

    public TeacherBuilder WithAddress(string address)
    {
        _dto.Address = address;
        return this;
    }

    public TeacherBuilder WithMobileNumber(string mobile, string code)
    {
        _dto.Mobile = new Mobile(code, mobile);
        return this;
    }

    public TeacherBuilder WithCourseId(int courseId)
    {
        _dto.CourseId = courseId;
        return this;
    }

    public TeacherBuilder WithDiploma(string diploma)
    {
        _dto.Diploma = diploma;
        return this;
    }

    public TeacherBuilder WithStudy(string study)
    {
        _dto.Study = study;
        return this;
    }

    public Teacher Build()
    {
        return _dto;
    }
}