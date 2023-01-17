using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.CourseServices.Contract;
using UnitSelection.Services.CourseServices.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.CourseTests.Delete;

public class FailedWhenThisCourseSelectedByStudent : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly CourseService _sut;
    private Course _course;
    private Term _term;
    private Class _class;
    private Func<Task> _actualResult;

    public FailedWhenThisCourseSelectedByStudent(ConfigurationFixture configuration) : base(configuration)
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
        var chooseUnit = new ChooseUnitBuilder()
            .WithCourseId(_course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(chooseUnit));
    }

    [BDDHelper.When("درسی با عنوان ‘ ریاضی مهندسی’ را حذف می کنیم.")]
    private async Task When()
    {
        _actualResult = async () => await _sut.Delete(_course.Id);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان " +
                    "‘درس انتخاب به دلیل انتخاب توسط دانشجو نمی توان حدف کرد’" +
                    " به کاربر نمایش می دهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<CourseSelectedByStudentException>();
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