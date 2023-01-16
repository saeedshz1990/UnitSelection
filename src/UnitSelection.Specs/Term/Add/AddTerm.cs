using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
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
        _dto =  new AddTermDtoBuilder()
            .WithName("مهرماه 1401")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();

        await _sut.Add(_dto);
    }

    [BDDHelper.Then("تنها یک ترم با عنوان ‘مهرماه’ در سیستم وجود دارد.")]
    public async Task Then()
    {
        var actualResult = await _context.Terms.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(_dto.Name);
        actualResult.StartDate.Should().Be(_dto.StartDate);
        actualResult.EndDate.Should().Be(_dto.EndDate);
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