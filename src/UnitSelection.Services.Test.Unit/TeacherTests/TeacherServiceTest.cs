using System.Diagnostics.Contracts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.TeacherServices.Contract;
using UnitSelection.Services.TeacherServices.Exceptions;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Services.Test.Unit.TeacherTests;

public class TeacherServiceTest
{
    private readonly EFDataContext _context;
    private readonly TeacherService _sut;

    public TeacherServiceTest()
    {
        _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
        _sut = TeacherServiceFactory.GenerateTeacherService(_context);
    }

    [Fact]
    public async Task Add_add_teacher_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder().Build();
        _context.Manipulate(_ => _.Add(course));
        var dto = new AddTeacherDtoBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithCourseid(course.Id)
            .Build();

        await _sut.Add(dto);

        var actualResult = await _context.Teachers.FirstOrDefaultAsync();
        actualResult!.FirstName.Should().Be(dto.FirstName);
        actualResult.LastName.Should().Be(dto.LastName);
        actualResult.FatherName.Should().Be(dto.FatherName);
        actualResult.NationalCode.Should().Be(dto.NationalCode);
        actualResult.Diploma.Should().Be(dto.Diploma);
        actualResult.Study.Should().Be(dto.Study);
        actualResult.DateOfBirth.Should().Be(dto.DateOfBirth);
        actualResult.Address.Should().Be(dto.Address);
        actualResult.GroupOfCourse.Should().Be(dto.GroupOfCourse);
    }

    [Fact]
    public async Task Add_throw_exception_When_teacher_name_is_exist_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder().Build();
        _context.Manipulate(_ => _.Add(course));
        var teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .Build();
        _context.Manipulate(_ => _.Add(teacher));
        var dto = new AddTeacherDtoBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithCourseid(course.Id)
            .Build();

        var actualResult = async () => await _sut.Add(dto);

        await actualResult.Should()
            .ThrowExactlyAsync<TeacherIsExistException>();
    }

    [Fact]
    public async Task Get_get_all_teacher_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder().Build();
        _context.Manipulate(_ => _.Add(course));
        var teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .Build();
        _context.Manipulate(_ => _.Add(teacher));

        _sut.GetAll();

        var actualResult = await _context.Teachers.ToListAsync();
        actualResult.Should().HaveCount(1);
    }

    [Fact]
    public async Task Get_get_all_teacher_when_not_any_teacher_added_properly()
    {
        _sut.GetAll();

        var actualResult = await _context.Teachers.ToListAsync();
        actualResult.Should().HaveCount(0);
    }

    [Fact]
    public async Task Get_get_by_id_teacher_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder().Build();
        _context.Manipulate(_ => _.Add(course));
        var teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .Build();
        _context.Manipulate(_ => _.Add(teacher));

        _sut.GetById(teacher.Id);

        var actualResult = await _context.Teachers.FirstOrDefaultAsync();
        actualResult!.FirstName.Should().Be(teacher.FirstName);
        actualResult.LastName.Should().Be(teacher.LastName);
        actualResult.FatherName.Should().Be(teacher.FatherName);
        actualResult.NationalCode.Should().Be(teacher.NationalCode);
        actualResult.Address.Should().Be(teacher.Address);
        actualResult.Diploma.Should().Be(teacher.Diploma);
        actualResult.Study.Should().Be(teacher.Study);
        actualResult.DateOfBirth.Should().Be(teacher.DateOfBirth);
        actualResult.CourseId.Should().Be(teacher.CourseId);
        actualResult.GroupOfCourse.Should().Be(teacher.GroupOfCourse);
    }

    [Fact]
    public async Task Get_get_by_course_id_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder().Build();
        _context.Manipulate(_ => _.Add(course));
        var teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .Build();
        _context.Manipulate(_ => _.Add(teacher));

        _sut.GetByCourseId(course.Id);

        var actualResult = await _context.Teachers.FirstOrDefaultAsync();
        actualResult!.FirstName.Should().Be(teacher.FirstName);
        actualResult.LastName.Should().Be(teacher.LastName);
        actualResult.FatherName.Should().Be(teacher.FatherName);
        actualResult.NationalCode.Should().Be(teacher.NationalCode);
        actualResult.Address.Should().Be(teacher.Address);
        actualResult.Diploma.Should().Be(teacher.Diploma);
        actualResult.Study.Should().Be(teacher.Study);
        actualResult.DateOfBirth.Should().Be(teacher.DateOfBirth);
    }

    [Fact]
    public async Task Delete_delete_teacher_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder().Build();
        _context.Manipulate(_ => _.Add(course));
        var teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .Build();
        _context.Manipulate(_ => _.Add(teacher));

        await _sut.Delete(teacher.Id);

        var actualResult = _context.Teachers.ToList();
        actualResult.Should().HaveCount(0);
    }

    [Theory]
    [InlineData(-1)]
    public async Task Delete_throw_exception_when_teacher_not_found_properly(int invalidId)
    {
        var actualResult = async () => await _sut.Delete(invalidId);

        await actualResult.Should()
            .ThrowExactlyAsync<TeacherNotFoundException>();
    }

    [Fact]
    public async Task Update_update_Teacher_properly()
    {
    }

    [Theory]
    [InlineData(-1)]
    public async Task Update_throw_exception_when_teacher_not_found_properly(int invalidId)
    {
        var dto = new UpdateTeacherDtoBuilder()
            .Build();
        var actualResult = async () => await _sut.Update(dto, invalidId);

        await actualResult.Should()
            .ThrowExactlyAsync<TeacherNotFoundException>();
    }

    [Fact]
    public async Task Update_throw_exception_teacher_is_exist_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder().Build();
        _context.Manipulate(_ => _.Add(course));
        var teacher = new TeacherBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(teacher));
        var secondTeacher = new TeacherBuilder()
            .WithFirstName("secondDummy")
            .WithLastName("secondLastDummy")
            .WithNationalCode("222222")
            .Build();
        _context.Manipulate(_ => _.Add(secondTeacher));

        var dto = new UpdateTeacherDtoBuilder()
            .WithFirstName("secondDummy")
            .WithLastName("secondLastDummy")
            .WithNationalCode("222222")
            .Build();

        var actualResult = async () => await _sut.Update(dto, teacher.Id);

        await actualResult.Should()
            .ThrowExactlyAsync<TeacherIsExistException>();
    }

    [Fact]
    public async Task Delete_throw_exception_when_student_selected_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("dummy", term.Id);
        _context.Manipulate(_ => _context.Add(newClass));
        var course = new CourseDtoBuilder().WithClassId(newClass.Id).Build();
        _context.Manipulate(_ => _context.Add(course));
        var teacher = new TeacherBuilder()
            .WithCourseId(course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(teacher));
        var chooseUnit = new ChooseUnitBuilder()
            .WithTeacherId(teacher.Id)
            .Build();
        _context.Manipulate(_ => _.Add(chooseUnit));
        
        var actualResult = async () => await _sut.Delete(teacher.Id);

        await actualResult.Should()
            .ThrowExactlyAsync<ThisTeacherSelectedByStudentException>();
    }
}