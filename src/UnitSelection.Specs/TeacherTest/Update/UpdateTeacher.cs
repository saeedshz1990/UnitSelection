using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.TeacherServices.Contract;
using UnitSelection.Services.TeacherServices.Contract.Dto;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.TeacherTest.Update;

public class UpdateTeacher : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TeacherService _sut;
    private Class _newClass;
    private Entities.Terms.Term _term;
    private Teacher _teacher;
    private Course _course;
    private UpdateTeacherDto _dto;

    public UpdateTeacher(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TeacherServiceFactory.GenerateTeacherService(_context);
    }

    [BDDHelper.Given("استادی با نام ‘آرش چناری’با مدرک تحصیلی" +
                     " ‘کارشناسی ارشد’ گرایش ‘مهدسی نرم افزار’" +
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

    [BDDHelper.When("استادی با نام ‘آرش چناری’ با مدرک تحصیلی" +
                    " ‘کارشناسی ارشد’ به گرایش ‘هوش مصنوعی’" +
                    " با کد ملی ‘2294321905’ ویرایش می کنم.")]
    private async Task When()
    {
        _dto = new UpdateTeacherDtoBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithDiploma("دکتری")
            .WithStudy("هوش مصنوعی")
            .WithCourseId(_course.Id)
            .Build();

        await _sut.Update(_dto, _teacher.Id);
    }

    [BDDHelper.Then("تنها یک استاد با نام ‘آرش چناری’ " +
                    "با مدرک تحصیلی ‘کارشناسی ارشد’" +
                    " به گرایش ‘هوش مصنوعی’ با کد ملی ‘2294321905’" +
                    " در سیتسم وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult = await _context.Teachers.FirstOrDefaultAsync();
        actualResult!.FirstName.Should().Be(_dto.FirstName);
        actualResult.LastName.Should().Be(_dto.LastName);
        actualResult.FatherName.Should().Be(_dto.FatherName);
        actualResult.Diploma.Should().Be(_dto.Diploma);
        actualResult.Study.Should().Be(_dto.Study);
        actualResult.DateOfBirth.Should().Be(_dto.DateOfBirth);
        actualResult.Address.Should().Be(_dto.Address);
        actualResult.GroupOfCourse.Should().Be(_dto.GroupOfCourse);
        actualResult.CourseId.Should().Be(_dto.CourseId);
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