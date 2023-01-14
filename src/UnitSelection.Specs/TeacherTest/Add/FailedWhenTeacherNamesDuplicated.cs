using FluentAssertions;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Teachers.Contract;
using UnitSelection.Services.Teachers.Contract.Dto;
using UnitSelection.Services.Teachers.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TeacherTestTools;
using Xunit;

namespace UnitSelection.Specs.TeacherTest.Add;

public class FailedWhenTeacherNamesDuplicated : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TeacherService _sut;
    private AddTeacherDto _dto;
    private Teacher _tetacher;
    private Func<Task> _actualResult;

    public FailedWhenTeacherNamesDuplicated(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TeacherServiceFactory.GenerateTeacherService(_context);
    }

    [BDDHelper.Given("استادی با نام ‘آرش چناری’با" +
                     " مدرک تحصیلی ‘کارشناسی ارشد’ گرایش" +
                     " ‘مهدسی نرم افزار’ با کد" +
                     " ملی ‘2294321905’ در سیستم وجود دارد.")]
    private void Given()
    {
        _tetacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .Build();
        _context.Manipulate(_ => _.Add(_tetacher));
    }

    [BDDHelper.When("یک استاد با نام ‘آرش چناری’با مدرک تحصیلی " +
                    "‘کارشناسی ارشد’ گرایش ‘مهدسی نرم افزار’ " +
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

        _actualResult = async () => await _sut.Add(_dto);
    }

    [BDDHelper.Then("پیغام خطایی با نام  ‘این استاد در سیستم" +
                    " وجود دارد’ به کاربر نمایش می دهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<TeacherIsExistException>();
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