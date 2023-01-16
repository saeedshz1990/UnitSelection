using FluentAssertions;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
using UnitSelection.Services.Terms.Exceptions;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.Term.Update;

public class FailedUpdateWhenNameEsExists : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TermService _sut;
    private UpdateTermDto _dto;
    private Entities.Terms.Term _firstTerm;
    private Entities.Terms.Term _secondTerm;
    private Func<Task> _actualResult;

    public FailedUpdateWhenNameEsExists(
        ConfigurationFixture configuration) 
        : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [BDDHelper.Given("ترمی با عنوان ترم ‘مهرماه’ وجود دارد.")]
    [BDDHelper.And("ترمی با عنوان ترم ‘بهمن ماه’ وجود دارد.")]
    public void Given()
    {
        _firstTerm = new TermBuilder()
            .WithName("مهرماه 1401")
            .WithStartDate(DateTime.UtcNow.AddMonths(3))
            .WithEndDate(DateTime.UtcNow.AddMonths(6))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(_firstTerm));
        _secondTerm = new TermBuilder()
            .WithName("بهمن ماه 1401")
            .WithStartDate(DateTime.UtcNow.AddMonths(3))
            .WithEndDate(DateTime.UtcNow.AddMonths(6))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(_secondTerm));
    }

    [BDDHelper.When("ترم ‘مهرماه’ را به ‘ بهمن ماه’ ویرایش می کنم")]
    public async Task When()
    {
        _dto = new UpdateTermDtoBuilder()
            .WithName("بهمن ماه 1401")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();
       
        _actualResult = async () => await _sut.Update(_firstTerm.Id, _dto);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان ‘نام ترم نمی تواند تکراری باشد’" +
                    " به کاربر نمایش می دهد.")]
    public async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<TheNameTermsCanNotRepeatedException>();
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