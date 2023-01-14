using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Teachers.Contract;
using UnitSelection.Services.Teachers.Contract.Dto;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TeacherTestTools;
using Xunit;

namespace UnitSelection.Specs.TeacherTest.Add;

public class AddTeacher : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TeacherService _sut;
    private AddTeacherDto _dto;

    public AddTeacher(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TeacherServiceFactory.GenerateTeacherService(_context);
    }

    [BDDHelper.Given("هیچ استادی در سیستم ثبت نشده است")]
    private void Given()
    {
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