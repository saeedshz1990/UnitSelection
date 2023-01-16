using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.CourseServices.Contract;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.CourseTests.Delete;

public class DeleteCourse :EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly CourseService _sut;
    private Course _course;
    private Entities.Terms.Term _term;
    private Class _class;
    
    public DeleteCourse(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = CourseServiceFactory.GenerateCourseService(_context);
    }
    
    [BDDHelper.Given("درسی با عنوان ‘ریاضی مهندسی’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(_term));
        _class = ClassFactory.GenerateClass("106", _term.Id);
        _context.Manipulate(_ => _.Add(_class));
        _course = new CourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_course));
    }

    [BDDHelper.When("درسی با عنوان ‘ ریاضی مهندسی’ را حذف می کنیم.")]
    private async Task When()
    {
        await _sut.Delete(_course.Id);
    }

    [BDDHelper.Then("بنابراین هیچ درسی با عنوان  ‘ریاضی مهندسی’ در سیستم وجود ندارد.")]
    private async Task Then()
    {
        var actualResult =await _context.Courses.ToListAsync();
        actualResult.Should().HaveCount(0);
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