using FluentAssertions;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
using UnitSelection.Services.Terms.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.Term.Add;

public class FailedWhenNameIsExist : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TermService _sut;
    private AddTermDto _dto;
    private Func<Task> _actualResult;

    public FailedWhenNameIsExist(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [BDDHelper.Given("ترمی با عنوان ترم ‘مهرماه’ وجود دارد.")]
    public void Given()
    {
        var term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_ => _.Terms.Add(term));
    }

    [BDDHelper.When("یک ترم با عنوان " +
                    "‘مهرماه’ در سیستم ثبت می کنم.")]
    public async Task When()
    {
        _dto = AddTermServiceFactory.GenerateAddTerm("مهرماه 1401");

        _actualResult = () => _sut.Add(_dto);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان ‘ نام ترم " +
                    "تکراری می باشد’ به کاربر نمایش می دهد.")]
    public async Task Then()
    {
        await _actualResult
            .Should()
            .ThrowExactlyAsync<TheTermsNameIsExistException>();
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