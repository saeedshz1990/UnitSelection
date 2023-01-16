using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
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

namespace UnitSelection.Specs.TeacherTest.Add;

public class AddTeacher : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TeacherService _sut;
    private Class _newClass;
    private Entities.Terms.Term _term;
    private Course _course;
    private AddTeacherDto _dto;

    public AddTeacher(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TeacherServiceFactory.GenerateTeacherService(_context);
    }

    [BDDHelper.Given("هیچ استادی در سیستم ثبت نشده است")]
    private void Given()
    {
         _term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(_term));
        _newClass = ClassFactory.GenerateClass("101", _term.Id);
        _context.Manipulate(_ => _context.Add(_newClass));
        _course = new CourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(3)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(_newClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_course));
    }

    [BDDHelper.When("یک استاد با نام ‘آرش چناری’با" +
                    " مدرک تحصیلی ‘کارشناسی ارشد’ گرایش" +
                    " ‘مهدسی نرم افزار’ " +
                    "با کد ملی ‘2294321905’ ثبت می نمایم. ")]
    private async Task When()
    {
        _dto = new AddTeacherDtoBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithCourseid(_course.Id)
            .Build();

        await _sut.Add(_dto);
    }

    [BDDHelper.Then("تنها یک استادی با نام " +
                    "‘ آرش چناری’ با مدرک تحصیلی ‘کارشناسی ارشد’ " +
                    "گرایش ‘مهدسی نرم افزار’ با کد ملی ‘2294321905’" +
                    " در سیستم باید وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult = await _context.Teachers.FirstOrDefaultAsync();
        actualResult!.FirstName.Should().Be(_dto.FirstName);
        actualResult.LastName.Should().Be(_dto.LastName);
        actualResult.FatherName.Should().Be(_dto.FatherName);
        actualResult.NationalCode.Should().Be(_dto.NationalCode);
        actualResult.Diploma.Should().Be(_dto.Diploma);
        actualResult.Study.Should().Be(_dto.Study);
        actualResult.DateOfBirth.Should().Be(_dto.DateOfBirth);
        actualResult.Address.Should().Be(_dto.Address);
        actualResult.GroupOfCourse.Should().Be(_dto.GroupOfCourse);
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