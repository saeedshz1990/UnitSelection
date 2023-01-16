using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.TeacherServices.Contract;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.TeacherTest.Delete;

public class DeleteTeacher : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TeacherService _sut;
    private Entities.Terms.Term _term;
    private Class _newClass;
    private Course _course;
    private Teacher _teacher;

    public DeleteTeacher(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TeacherServiceFactory.GenerateTeacherService(_context);
    }
    
    [BDDHelper.Given("استادی با نام ‘آرش چناری’با مدرک تحصیلی ‘" +
                     "کارشناسی ارشد’ گرایش ‘مهدسی نرم افزار’" +
                     " با کد ملی ‘2294321905’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(_term));
        _newClass = ClassFactory.GenerateClass("101", _term.Id);
        _context.Manipulate(_ => _context.Add(_newClass));
        _course = new CourseDtoBuilder().WithClassId(_newClass.Id).Build();
        _context.Manipulate(_ => _context.Add(_course));
        _teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithCourseId(_course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_teacher));
    }

    [BDDHelper.When("استادی با نام ‘آرش چناری’با مدرک تحصیلی" +
                    " ‘کارشناسی ارشد’ گرایش ‘مهدسی نرم افزار’" +
                    " با کد ملی ‘2294321905’ را حذف می کنیم.")]
    private async Task When()
    {
        await _sut.Delete(_teacher.Id);
    }

    [BDDHelper.Then("نباید هیچ استادی در سیستم وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult = _context.Teachers.ToList();
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