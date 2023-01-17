using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.CourseServices.Contract;
using UnitSelection.Services.CourseServices.Contract.Dto;
using UnitSelection.Services.CourseServices.Exceptions;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Services.Test.Unit.CourseTests;

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

    [Fact]
    public async Task Update_update_course_properly()
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
        var dto = new UpdateCourseDto
        {
            Name = "مهندسی نرم افزار",
            UnitCount = 3,
            DayOfWeek = "یکشنبه",
            StartHour = "08:00",
            EndHour = "10:00",
            ClassId = newClass.Id
        };

        await _sut.Update(dto, newClass.Id);

        var actualResult = await _context.Courses.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(dto.Name);
        actualResult.StartHour.Should().Be(dto.StartHour);
        actualResult.EndHour.Should().Be(dto.EndHour);
        actualResult.UnitCount.Should().Be(dto.UnitCount);
        actualResult.DayOfWeek.Should().Be(dto.DayOfWeek);
        actualResult.ClassId.Should().Be(dto.ClassId);
    }

    [Theory]
    [InlineData(-1)]
    public async Task Update_throw_exception_When_course_notfound_properly(int invalidId)
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var dto = new UpdateCourseDto
        {
            Name = "مهندسی نرم افزار",
            UnitCount = 3,
            DayOfWeek = "یکشنبه",
            StartHour = "08:00",
            EndHour = "10:00",
            ClassId = newClass.Id
        };
        var actualResult = async () => await _sut.Update(dto, invalidId);

        await actualResult.Should()
            .ThrowExactlyAsync<CourseNotFoundException>();
    }

    [Fact]
    public async Task Update_throw_exception_when_course_name_is_duplicated_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var firstCourse = new CourseDtoBuilder().Build();
        _context.Manipulate(_ => _.Add(firstCourse));
        var secondCourse = new CourseDtoBuilder()
            .WithName("secondDummy")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(secondCourse));
        var dto = new UpdateCourseDtoBuilder()
            .WithName("secondDummy")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();

        var actualResult = async () => await _sut.Update(dto, firstCourse.Id);

        await actualResult.Should()
            .ThrowExactlyAsync<TheCourseNameWithSameTeacherException>();
    }

    [Fact]
    public async Task update_throw_exception_when_course_unit_count_equal_or_lower_than__zero_properly()
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
        var dto = new UpdateCourseDto
        {
            Name = "dummy",
            UnitCount = 0,
            DayOfWeek = "یکشنبه",
            StartHour = "08:00",
            EndHour = "10:00",
            ClassId = newClass.Id
        };

        var actualResult = async () => await _sut.Update(dto, newClass.Id);

        await actualResult.Should()
            .ThrowExactlyAsync<CourseUnitCountCanNotBeZeroException>();
    }

    [Fact]
    public async Task Get_get_all_course_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var firstDummy = new CourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(firstDummy));
        var secondDummy = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(secondDummy));

        _sut.GetAll();

        var actualResult = await _context.Courses.ToListAsync();
        actualResult.Should().HaveCount(2);
    }

    [Fact]
    public async Task Get_get_all_when_not_any_courses_added_properly()
    {
        _sut.GetAll();

        var actualResult = await _context.Courses.ToListAsync();
        actualResult.Should().HaveCount(0);
    }

    [Fact]
    public async Task Get_get_by_id_course_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var firstDummy = new CourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(firstDummy));
        var secondDummy = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(secondDummy));

        _sut.GetById(secondDummy.Id);

        var actualResult = await _context.Courses.FirstOrDefaultAsync(_ => _.Id == secondDummy.Id);
        actualResult!.Name.Should().Be(secondDummy.Name);
        actualResult!.DayOfWeek.Should().Be(secondDummy.DayOfWeek);
        actualResult!.UnitCount.Should().Be(secondDummy.UnitCount);
        actualResult!.StartHour.Should().Be(secondDummy.StartHour);
        actualResult!.EndHour.Should().Be(secondDummy.EndHour);
        actualResult!.ClassId.Should().Be(secondDummy.ClassId);
    }

    [Fact]
    public async Task Get_get_by_class_id_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _.Add(newClass));
        var secondClass = ClassFactory.GenerateClass("102", term.Id);
        _context.Manipulate(_ => _.Add(secondClass));
        var firstDummy = new CourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(firstDummy));
        var secondDummy = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(secondClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(secondDummy));

        _sut.GetByClassId(secondClass.Id);

        var actualResult = await _context.Courses.FirstOrDefaultAsync(_ => _.ClassId == secondClass.Id);
        actualResult!.Name.Should().Be(secondDummy.Name);
        actualResult.DayOfWeek.Should().Be(secondDummy.DayOfWeek);
        actualResult.UnitCount.Should().Be(secondDummy.UnitCount);
        actualResult.StartHour.Should().Be(secondDummy.StartHour);
        actualResult.EndHour.Should().Be(secondDummy.EndHour);
    }

    [Fact]
    public async Task Delete_delete_course_properly()
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

        await _sut.Delete(course.Id);

        var actualResult = await _context.Courses.ToListAsync();
        actualResult.Should().HaveCount(0);
    }

    [Theory]
    [InlineData(-1)]
    public async Task Delete_throw_exception_when_course_notFound_properly(int invalidId)
    {
        var actualResult = async () => await _sut.Delete(invalidId);
        
        await actualResult.Should()
            .ThrowExactlyAsync<CourseNotFoundException>();
    }

    [Fact]
    public async Task Delete_throw_exception_when_course_selected_by_student_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newclass = ClassFactory.GenerateClass("106", term.Id);
        _context.Manipulate(_ => _.Add(newclass));
        var course = new CourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newclass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(course));
        var chooseUnit = new ChooseUnitBuilder()
            .WithCourseId(course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(chooseUnit));
        
        var actualResult = async () => await _sut.Delete(course.Id);
        
        await actualResult.Should()
            .ThrowExactlyAsync<CourseSelectedByStudentException>();
    }
}