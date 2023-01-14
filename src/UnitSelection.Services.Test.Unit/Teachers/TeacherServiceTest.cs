using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Teachers.Contract;
using UnitSelection.Services.Teachers.Exceptions;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Services.Test.Unit.Teachers;

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
}