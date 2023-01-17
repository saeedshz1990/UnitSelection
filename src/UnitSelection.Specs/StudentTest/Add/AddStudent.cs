using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Contracts.Dto;
using UnitSelection.TestTools.StudentTestTools;
using Xunit;

namespace UnitSelection.Specs.StudentTest.Add;

public class AddStudent : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly StudentService _sut;
    private AddStudentDto _dto;

    public AddStudent(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = StudentServiceFactory.GenerateStudentService(_context);
    }

    [BDDHelper.Given("هیچ دانشجویی در سیستم ثبت نشده است")]
    private void Given()
    {
    }

    [BDDHelper.When("یک دانشجو با نام ‘ سعید انصاری’" +
                    " به تاریخ تولد ‘1369’" +
                    " و شماره شناسنامه ‘2280509504 ‘ با " +
                    " در سیستم ثبت می کنم.")]
    private async Task When()
    {
        _dto = new AddStudentDtoBuilder()
            .WithFirstName("سعید")
            .WithLastName("انصاری")
            .WithFatherName("محمدجواد")
            .WithDateOfBirth("1369")
            .WithNationalCode("2280509504")
            .WithMobileNumber("9177877225", "98")
            .Build();

        await _sut.Add(_dto);
    }

    [BDDHelper.Then("تنها یک دانشجو با نام" +
                    " ‘ سعید انصاری’ به تاریخ تولد ‘1369’ و " +
                    "شماره شناسنامه ‘2280509504 ‘ با رشته تحصیلی" +
                    " ‘کامپیوتر’ باید وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult = await _context.Students.FirstOrDefaultAsync();
        actualResult!.FirstName.Should().Be(_dto.FirstName);
        actualResult.LastName.Should().Be(_dto.LastName);
        actualResult.FatherName.Should().Be(_dto.FatherName);
        actualResult.Address.Should().Be(_dto.Address);
        actualResult.NationalCode.Should().Be(_dto.NationalCode);
        actualResult.DateOfBirth.Should().Be(_dto.DateOfBirth);
        actualResult.Mobile.MobileNumber.Should().Be(_dto.Mobile.MobileNumber);
        actualResult.Mobile.CountryCallingCode.Should().Be(_dto.Mobile.CountryCallingCode);
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