using FluentAssertions;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.Term.Delete;

public class DeleteTerm : EFDataContextDatabaseFixture
{
    private readonly TermService _sut;
    private readonly EFDataContext _context;
    private Entities.Terms.Term _term;

    public DeleteTerm(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [BDDHelper.Given("ترمی با عنوان ترم ‘مهرماه’ وجود دارد.")]
    public void Given()
    {
        _term = TermServiceFactoryDto.GenerateTerms("مهرماه");
        _context.Manipulate(_ => _.Terms.Add(_term));
    }

    [BDDHelper.When("ترمی با عنوان ‘مهرماه’ را حذف می کنم.")]
    public async Task When()
    {
        _sut.Delete(_term.Id);
    }

    [BDDHelper.Then("هیچ ترمی با عنوان ‘مهرماه’ " +
                    "نباید در سیستم وجود داشته باشد.")]
    public async Task Then()
    {
        var actualResult = _context.Terms.ToList();
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