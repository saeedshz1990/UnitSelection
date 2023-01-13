using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Courses;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Courses.Contract;
using UnitSelection.Services.Courses.Contract.Dto;
using UnitSelection.Services.Courses.Exceptions;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Services.Test.Unit.Courses;

public class CourseServiceTest
{
    private readonly EFDataContext _context;
    private readonly CourseService _sut;

    public CourseServiceTest()
    {
        _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
        _sut = CourseServiceFactory.GenerateCourseService(_context);
    }

    [Fact]
    public async Task Add_add_Course_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _context.Add(newClass));

        var dto = new AddCourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();

        await _sut.Add(dto);

        var actualResult = await _context.Courses.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(dto.Name);
        actualResult.StartHour.Should().Be(dto.StartHour);
        actualResult.EndHour.Should().Be(dto.EndHour);
        actualResult.UnitCount.Should().Be(dto.UnitCount);
        actualResult.DayOfWeek.Should().Be(dto.DayOfWeek);
        actualResult.ClassId.Should().Be(dto.ClassId);
    }

    [Fact]
    public async Task Add_throw_exception_when_course_name_is_repeated_for_same_teacher()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(course));

        var dto = new AddCourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();

        var actualResult = async () => await _sut.Add(dto);

        await actualResult.Should()
            .ThrowExactlyAsync<TheCourseNameWithSameTeacherException>();
    }

    [Theory]
    [InlineData(-1)]
    public async Task 
        Add_throw_exception_when_course_unit_count_equal_or_lower_than_zero_properly(int invalidCount)
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));

        var dto = new AddCourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(invalidCount)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();

        var actualResult = async () => await _sut.Add(dto);

        await actualResult.Should()
            .ThrowExactlyAsync<CourseUnitCountCanNotBeZeroException>();
    }
}