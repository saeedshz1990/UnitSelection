using UnitSelection.Entities;
using UnitSelection.Entities.Teachers;

namespace UnitSelection.TestTools.TeacherTestTools;

public class TeacherBuilder
{
    private Teacher _teacher;

    public TeacherBuilder()
    {
        _teacher = new Teacher
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
        _teacher.FirstName = firstName;
        return this;
    }

    public TeacherBuilder WithLastName(string lastName)
    {
        _teacher.LastName = lastName;
        return this;
    }

    public TeacherBuilder WithFatherName(string fatherName)
    {
        _teacher.FatherName = fatherName;
        return this;
    }

    public TeacherBuilder WithNationalCode(string code)
    {
        _teacher.NationalCode = code;
        return this;
    }

    public TeacherBuilder WithDateOfBirth(string birth)
    {
        _teacher.DateOfBirth = birth;
        return this;
    }

    public TeacherBuilder WithGroupOfCourse(string groupCourse)
    {
        _teacher.GroupOfCourse = groupCourse;
        return this;
    }

    public TeacherBuilder WithAddress(string address)
    {
        _teacher.Address = address;
        return this;
    }

    public TeacherBuilder WithMobileNumber(string mobile, string code)
    {
        _teacher.Mobile = new Mobile(code, mobile);
        return this;
    }

    public TeacherBuilder WithCourseId(int courseId)
    {
        _teacher.CourseId = courseId;
        return this;
    }

    public TeacherBuilder WithDiploma(string diploma)
    {
        _teacher.Diploma = diploma;
        return this;
    }

    public TeacherBuilder WithStudy(string study)
    {
        _teacher.Study = study;
        return this;
    }

    public Teacher Build()
    {
        return _teacher;
    }
}