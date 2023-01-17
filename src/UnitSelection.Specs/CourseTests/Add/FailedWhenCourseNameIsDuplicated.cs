using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.CourseServices.Contract;
using UnitSelection.Services.CourseServices.Contract.Dto;
using UnitSelection.Services.CourseServices.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.CourseTests.Add;

public class FailedWhenCourseNameIsDuplicated : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly CourseService _sut;
    private AddCourseDto _dto;
    private Course _course;
    private Term _term;
    private Class _class;
    private Func<Task> _actualResult;

    public FailedWhenCourseNameIsDuplicated(
        ConfigurationFixture configuration)
        : base(configuration)
    {
        _context = CreateDataContext();
        _sut = CourseServiceFactory.GenerateCourseService(_context);
    }

    [BDDHelper.Given("درسی با عنوان ‘ریاضی مهندسی ‘" +
                     " در سیستم موجود می باشد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(_term));
        _class = ClassFactory.GenerateClass("101", _term.Id);
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

    [BDDHelper.When("یک درس با عنوان ‘ریاضی مهندسی’ " +
                    "در سیستم ثبت می کنم.")]
    private async Task When()
    {
        _dto = new AddCourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(_class.Id)
            .Build();

        _actualResult = async () => await _sut.Add(_dto);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان ‘نام درس تکراری می باشد’" +
                    " به کاربر نمایش می دهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<TheCourseNameWithSameTeacherException>();
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