using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.CourseServices.Contract;
using UnitSelection.Services.CourseServices.Contract.Dto;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.CourseTests.Update;

public class UpdateCourse : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly CourseService _sut;
    private UpdateCourseDto _dto;
    private Course _course;
    private Term _term;
    private Class _class;

    public UpdateCourse(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = CourseServiceFactory.GenerateCourseService(_context);
    }

    [BDDHelper.Given("یک درس با عنوان ‘ریاضی مهندسی’ در سیستم وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(_term));
        _class = ClassFactory.GenerateClass("101", _term.Id);
        _context.Manipulate(_ => _.Add(_class));
        _course = new CourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_course));
    }

    [BDDHelper.When("درسی با عنوان ‘ریاضی مهندسی ‘ را به " +
                    "‘مهندسی نرم افزار’ ویرایش می کنم.")]
    private async Task When()
    {
        _dto = new UpdateCourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("08:00")
            .WithEndHour("10:00")
            .WithClassId(_class.Id)
            .Build();

        await _sut.Update(_dto, _course.Id);
    }

    [BDDHelper.Then("تنها یک درس با عنوان" +
                    " ‘مهندسی نرم افزار’ در سیستم وجود دارد.")]
    private async Task Then()
    {
        var actualResult = await _context.Courses.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(_dto.Name);
        actualResult.StartHour.Should().Be(_dto.StartHour);
        actualResult.EndHour.Should().Be(_dto.EndHour);
        actualResult.UnitCount.Should().Be(_dto.UnitCount);
        actualResult.DayOfWeek.Should().Be(_dto.DayOfWeek);
        actualResult.ClassId.Should().Be(_dto.ClassId);
    }

    [Fact]
    public void Run()
    {
        BDDHelper.Runner.RunScenario(
            _ => Given(),
            _ => When().Wait(),
            _ => Then().Wait()
        );
    }
}