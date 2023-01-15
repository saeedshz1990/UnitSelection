using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Contracts.Dto;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.StudentTestTools;
using Xunit;

namespace UnitSelection.Specs.StudentTest.Update;

public class UpdateStudent:EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly StudentService _sut;
    private UpdateStudentDto _dto;
    private Student _student;
    
    public UpdateStudent(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = StudentServiceFactory.GenerateStudentService(_context);
    }
    
    [BDDHelper.Given("یک دانشجو با نام" +
                     " ‘ سعید انصاری’ به تاریخ تولد ‘1369’" +
                     " و شماره شناسنامه ‘2280509504 ‘ " +
                     "با رشته تحصیلی ‘کامپیوتر’ وجود دارد.")]
    private void Given()
    {
        _student = new StudentBuilder()
            .WithFirstName("سعید")
            .WithLastName("انصاری")
            .WithFatherName("محمدجواد")
            .WithDateOfBirth("1369")
            .WithNationalCode("2280509504")
            .WithMobileNumber("9177877225", "98")
            .Build();
        _context.Manipulate(_ => _.Add(_student));
    }

    [BDDHelper.When("نام دانشجو به " +
                    "‘محمدرضا انصاری’ به تاریخ تولد ‘1369’" +
                    " و شماره شناسنامه ‘2280509504 ‘ " +
                    "با رشته تحصیلی ‘کامپیوتر’ ویرایش می کنم.")]
    private async Task When()
    {
        _dto=new UpdateStudentDtoBuilder()
            .WithFirstName("محمدرضا")
            .WithLastName("انصاری")
            .WithFatherName("محمدجواد")
            .WithDateOfBirth("1369")
            .WithMobileNumber("9177877225", "98")
            .Build();

        await _sut.Update(_dto,_student.Id);
    }

    [BDDHelper.Then("تنها یک دانشجو با نام " +
                    " ‘محمدرضا انصاری’ به تاریخ تولد ‘1369’" +
                    " و شماره شناسنامه ‘2280509504 ‘ " +
                    "با رشته تحصیلی ‘کامپیوتر’ در سیستم موجود می باشد.")]
    private async Task Then()
    {
        var actualResult = await _context
            .Students.FirstOrDefaultAsync();
        actualResult!.FirstName.Should().Be(_dto.FirstName);
        actualResult.LastName.Should().Be(_dto.LastName);
        actualResult.FatherName.Should().Be(_dto.FatherName);
        actualResult.Address.Should().Be(_dto.Address);
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