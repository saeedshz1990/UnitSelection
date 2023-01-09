using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.Term.Add;

[BDDHelper.Scenario("تعریف ترم")]
public class AddTerm : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TermService _sut;
    private AddTermDto _dto;


    public AddTerm(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [BDDHelper.Given("هیچ ترمی در سیستم وجود ندارد.")]
    public void Given()
    {
    }

    [BDDHelper.When("یک ترم با عنوان ترم ‘مهرماه’ به سیستم اضافه می کنم.")]
    public async Task When()
    {
        _dto = AddTermServiceFactory.GenerateAddTerm("مهرماه 1401");

        await _sut.Add(_dto);
    }

    [BDDHelper.Then("تنها یک ترم با عنوان ‘مهرماه’ در سیستم وجود دارد.")]
    public async Task Then()
    {
        var actualResult = await _context.Terms.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(_dto.Name);
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