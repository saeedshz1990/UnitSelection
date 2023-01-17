using FluentAssertions;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
using UnitSelection.Services.Terms.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.TermTest.Update;

public class FailedWhenEndDateEqualOrLowerThanStartDate : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TermService _sut;
    private Term _term;
    private UpdateTermDto _dto;
    private Func<Task> _actualResult;

    public FailedWhenEndDateEqualOrLowerThanStartDate(
        ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [BDDHelper.Given("ترمی با عنوان ترم ‘مهرماه’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder()
            .WithName("مهرماه 1401")
            .WithStartDate(DateTime.UtcNow)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(_term));
    }

    [BDDHelper.When("ترم ‘مهرماه’ را به ‘ بهمن ماه’ ویرایش می کنم")]
    private async Task When()
    {
        _dto = new UpdateTermDtoBuilder()
            .WithName("بهمن ماه 1401")
            .WithStartDate(DateTime.UtcNow.AddDays(1))
            .WithEndDate(DateTime.UtcNow.Date)
            .Build();

        _actualResult = async () => await _sut.Update(_term.Id, _dto);
    }
    
    [BDDHelper.Then("پیغام خطایی با عنوان ‘نام ترم نمی تواند تکراری باشد’" +
                    " به کاربر نمایش می دهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<
                TheEndDateTermsCanNotLowerThanOrEqualStartDateException>();
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