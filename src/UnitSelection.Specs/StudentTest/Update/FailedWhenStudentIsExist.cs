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

namespace UnitSelection.Specs.StudentTest.Update;

public class FailedWhenStudentIsExist : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly StudentService _sut;
    private UpdateStudentDto _dto;
    private Student _student;
    private Student _secondStudent;
    private Func<Task> _actualResult;

    public FailedWhenStudentIsExist(ConfigurationFixture configuration)
        : base(configuration)
    {
        _context = CreateDataContext();
        _sut = StudentServiceFactory.GenerateStudentService(_context);
    }

    [BDDHelper.Given("دانشجویی با نام ‘ سعید انصاری’" +
                     " با تاریخ تولد ‘1369’" +
                     " و شماره شناسنامه ‘2280509504 ‘" +
                     " وجود دارد .")]
    [BDDHelper.And("دانشجویی با نام" +
                   " ‘ حسین محمدیان’ با تاریخ تولد ‘1380’" +
                   " و شماره شناسنامه ‘2291006572 ‘ " +
                   "با رشته تحصیلی ‘کامپیوتر’ در سیستم وجود دارد .")]
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
        _secondStudent = new StudentBuilder()
            .WithFirstName("حسین")
            .WithLastName("محمدیان")
            .WithFatherName("علیرضا")
            .WithDateOfBirth("1369")
            .WithNationalCode("2291006572")
            .WithMobileNumber("9177427225", "98")
            .Build();
        _context.Manipulate(_ => _.Add(_secondStudent));
    }

    [BDDHelper.When("نام دانشجو به ‘سعید انصاری’" +
                    " به تاریخ تولد ‘1369’" +
                    " و شماره شناسنامه ‘2291006572 ‘ " +
                    "با رشته تحصیلی ‘کامپیوتر’ ویرایش می کنم.")]
    private async Task When()
    {
        _dto = new UpdateStudentDtoBuilder()
            .WithFirstName("محمدرضا")
            .WithLastName("انصاری")
            .WithFatherName("محمدجواد")
            .WithDateOfBirth("1369")
            .Build();

        _actualResult = async () => await _sut.Update(_dto, _student.Id);
    }

    [BDDHelper.Then("یک پیغام خطا با نام " +
                    "‘دانشجو در سیستم وجود دارد’ به کاربر نمایش دهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<StudentIsExistException>();
    }

    [Fact(Skip = "Not implementing")]
    public void Run()
    {
        BDDHelper.Runner.RunScenario(
            _ => Given(),
            _ => When().Wait(),
            _ => Then().Wait()
        );
    }
}