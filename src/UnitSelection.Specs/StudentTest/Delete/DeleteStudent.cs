using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.StudentTestTools;
using Xunit;

namespace UnitSelection.Specs.StudentTest.Delete;

public class DeleteStudent : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly StudentService _sut;
    private Student _student;

    public DeleteStudent(ConfigurationFixture configuration)
        : base(configuration)
    {
        _context = CreateDataContext();
        _sut = StudentServiceFactory.GenerateStudentService(_context);
    }

    [BDDHelper.Given("یک دانشجو با نام ‘ سعید انصاری’" +
                     " به تاریخ تولد ‘1369’ " +
                     "و شماره شناسنامه" +
                     " ‘2280509504 ‘ وجود دارد.")]
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

    [BDDHelper.When("دانشجو با نام ‘ سعید انصاری’" +
                    " به تاریخ تولد ‘1369’ " +
                    "و شماره شناسنامه ‘2280509504 ‘" +
                    "از سیستم حذف می کنم")]
    private async Task When()
    {
        await _sut.Delete(_student.Id);
    }

    [BDDHelper.Then("نباید دانشجویی با نام ‘ سعید انصاری’" +
                    " به تاریخ تولد ‘1369’ " +
                    "و شماره شناسنامه ‘2280509504 ‘" +
                    "در سیستم وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult = await _context.Students.ToListAsync();
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