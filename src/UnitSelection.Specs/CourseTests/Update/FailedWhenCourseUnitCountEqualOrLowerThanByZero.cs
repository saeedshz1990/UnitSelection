using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.CourseServices.Contract;
using UnitSelection.Services.CourseServices.Contract.Dto;
using UnitSelection.Services.CourseServices.Exceptions;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.CourseTests.Update;

public class FailedWhenCourseUnitCountEqualOrLowerThanByZero : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly CourseService _sut;
    private UpdateCourseDto _dto;
    private Course _course;
    private Entities.Terms.Term _term;
    private Class _class;
    private Func<Task> _actualResult;

    public FailedWhenCourseUnitCountEqualOrLowerThanByZero(
        ConfigurationFixture configuration) :
        base(configuration)
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
            .WithUnitCount(3)
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_course));
    }

    [BDDHelper.When("درس ریاضی مهندسی با را با تعداد واحد 3 به 0 تغییر می دهم")]
    private async Task When()
    {
        _dto = new UpdateCourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(0)
            .WithStartHour("08:00")
            .WithEndHour("10:00")
            .WithClassId(_class.Id)
            .Build();

        _actualResult = async () => await _sut.Update(_dto, _course.Id);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان ' تعداد واحد نم یتواند صفر باشد' به کاربر نمایش می دهد")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<CourseUnitCountCanNotBeZeroException>();
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