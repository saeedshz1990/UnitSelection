using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.CourseServices.Contract;
using UnitSelection.Services.CourseServices.Contract.Dto;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.CourseTests.Add;

public class AddCourse : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly CourseService _sut;
    private AddCourseDto _dto;

    public AddCourse(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = CourseServiceFactory.GenerateCourseService(_context);
    }

    [BDDHelper.Given("هیچ درسی در سیستم ثبت نشده است")]
    private void Given()
    {
    }

    [BDDHelper.When("یک درس با عنوان ‘ریاضی مهندسی’ در سیستم ثبت می کنم.")]
    private async Task When()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _context.Add(newClass));

        _dto = new AddCourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();

        await _sut.Add(_dto);
    }

    [BDDHelper.Then("بنابراین یک درس با " +
                    "عنوان ‘ریاضی مهندسی ‘ " +
                    "باید در سیستم وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult = await _context.Courses.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(_dto.Name);
        actualResult!.StartHour.Should().Be(_dto.StartHour);
        actualResult!.EndHour.Should().Be(_dto.EndHour);
        actualResult!.UnitCount.Should().Be(_dto.UnitCount);
        actualResult!.DayOfWeek.Should().Be(_dto.DayOfWeek);
        actualResult!.ClassId.Should().Be(_dto.ClassId);
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