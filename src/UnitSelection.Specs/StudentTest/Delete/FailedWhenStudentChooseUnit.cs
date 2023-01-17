using FluentAssertions;
using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Exceptions;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.StudentTestTools;
using Xunit;

namespace UnitSelection.Specs.StudentTest.Delete;

public class FailedWhenStudentChooseUnit : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly StudentService _sut;
    private Student _student;
    private ChooseUnit _chooseUnit;
    private Func<Task> _actualResult;

    public FailedWhenStudentChooseUnit(
        ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = StudentServiceFactory.GenerateStudentService(_context);
    }

    [BDDHelper.Given("یک دانشجو با نام ‘ سعید انصاری’ به تاریخ تولد" +
                     " ‘1369’ و شماره شناسنامه ‘2280509504 ‘" +
                     "  وجود دارد.")]
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

        _chooseUnit = new ChooseUnitBuilder()
            .WithStudentId(_student.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_chooseUnit));
    }

    [BDDHelper.When("دانشجو با نام ‘سعید انصاری’ " +
                    "به تاریخ تولد ‘1369’ و" +
                    " شماره شناسنامه ‘2280509504 ‘ با رشته " +
                    "تحصیلی ‘کامپیوتر’ از سیستم حذف می کنم")]
    private async Task When()
    {
        _actualResult = async () => await _sut.Delete(_student.Id);
    }

    [BDDHelper.Then("یک پیغام خطا با نام " +
                    "‘دانشجو در سیستم انتخاب واحد انجام داده است’" +
                    " به کاربر نمایش دهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<StudentHaveChooseUnitException>();
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