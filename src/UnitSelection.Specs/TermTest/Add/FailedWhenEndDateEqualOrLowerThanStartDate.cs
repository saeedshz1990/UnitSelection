using FluentAssertions;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
using UnitSelection.Services.Terms.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.TermTest.Add;

public class FailedWhenEndDateEqualOrLowerThanStartDate : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TermService _sut;
    private AddTermDto _dto;
    private Func<Task> _actualResult;

    public FailedWhenEndDateEqualOrLowerThanStartDate(
        ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [BDDHelper.Given("هیچ ترمی در سیستم وجود ندارد.")]
    private void Given()
    {
    }

    [BDDHelper.When("یک ترم با عنوان ترم ‘مهرماه’ به سیستم اضافه می کنم.")]
    private async Task When()
    {
        _dto = new AddTermDtoBuilder()
            .WithName("مهرماه 1401")
            .WithStartDate(DateTime.UtcNow.Date.AddDays(1))
            .WithEndDate(DateTime.UtcNow.Date)
            .Build();

        _actualResult = async () => await _sut.Add(_dto);
    }
    
    [BDDHelper.Then("پیغام خطایی با عنوان ‘ نام ترم " +
                    "تکراری می باشد’ به کاربر نمایش می دهد.")]
    public async Task Then()
    {
        await _actualResult
            .Should()
            .ThrowExactlyAsync<TheEndDateTermsCanNotLowerThanOrEqualStartDateException>();
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