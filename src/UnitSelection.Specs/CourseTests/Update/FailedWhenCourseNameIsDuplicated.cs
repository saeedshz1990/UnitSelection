using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Courses.Contract;
using UnitSelection.Services.Courses.Contract.Dto;
using UnitSelection.Services.Courses.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.CourseTests.Update;

public class FailedWhenCourseNameIsDuplicated : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly CourseService _sut;
    private UpdateCourseDto _dto;
    private Course _firstCourse;
    private Course _secondCourse;
    private Entities.Terms.Term _term;
    private Class _class;
    private Func<Task> _actualResult;

    public FailedWhenCourseNameIsDuplicated(
        ConfigurationFixture configuration)
        : base(configuration)
    {
        _context = CreateDataContext();
        _sut = CourseServiceFactory.GenerateCourseService(_context);
    }

    [BDDHelper.Given("یک درس با عنوان ‘ریاضی مهندسی’ وجود دارد.")]
    [BDDHelper.And("یک درس با عنوان ‘مهندسی نرم افزار’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(_term));
        _class = ClassFactory.GenerateClass("101", _term.Id);
        _context.Manipulate(_ => _.Add(_class));
        _firstCourse = new CourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_firstCourse));
        _secondCourse = new CourseDtoBuilder()
            .WithName("مهدسی نرم افزار")
            .WithDayOfWeek("دوشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_secondCourse));
    }

    [BDDHelper.When("درسی با عنوان ‘ریاضی مهندسی ‘ " +
                    "را به ‘مهندسی نرم افزار’ ویرایش می کنم.")]
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

        _actualResult = async () => await _sut.Update(_dto, _firstCourse.Id);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان" +
                    "  ‘ نام درس تکراری می باشد’" +
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