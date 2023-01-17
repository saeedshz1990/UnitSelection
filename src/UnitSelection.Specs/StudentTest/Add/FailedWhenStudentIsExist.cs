using FluentAssertions;
using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Contracts.Dto;
using UnitSelection.Services.StudentServices.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.StudentTestTools;
using Xunit;

namespace UnitSelection.Specs.StudentTest.Add;

public class FailedWhenStudentIsExist : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly StudentService _sut;
    private AddStudentDto _dto;
    private Student _student;
    private Func<Task> _actualResult;

    public FailedWhenStudentIsExist(
        ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = StudentServiceFactory.GenerateStudentService(_context);
    }

    [BDDHelper.Given("دانشجویی با نام ‘ سعید انصاری’ به " +
                     "تاریخ تولد ‘1369’ و شماره شناسنامه ‘2280509504 ‘" +
                     " وجود دارد.")]
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

    [BDDHelper.When("یک دانشجو با نام" +
                    " ‘ سعید انصاری’ " +
                    "به تاریخ تولد ‘1369’ و " +
                    "شماره شناسنامه ‘2280509504 ‘ " +
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

        _actualResult = async () => await _sut.Add(_dto);
    }

    [BDDHelper.Then("یک پیغام خطا با نام ‘دانشجو در سیستم وجود دارد’ به کاربر نمایش دهد.")]
    private async Task Then()
    {
        await _actualResult.Should().ThrowExactlyAsync<StudentIsExistException>();
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