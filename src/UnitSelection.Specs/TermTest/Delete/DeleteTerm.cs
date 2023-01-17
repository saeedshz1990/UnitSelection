using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.TermTest.Delete;

public class DeleteTerm : EFDataContextDatabaseFixture
{
    private readonly TermService _sut;
    private readonly EFDataContext _context;
    private Term _term;

    public DeleteTerm(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [BDDHelper.Given("ترمی با عنوان ترم ‘مهرماه’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder()
            .WithName("مهرماه 1401")
            .WithStartDate(DateTime.UtcNow.AddMonths(3))
            .WithEndDate(DateTime.UtcNow.AddMonths(6))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(_term));
    }

    [BDDHelper.When("ترمی با عنوان ‘مهرماه’ را حذف می کنم.")]
    private async Task When()
    {
       await _sut.Delete(_term.Id);
    }

    [BDDHelper.Then("هیچ ترمی با عنوان ‘مهرماه’ " +
                    "نباید در سیستم وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult =await _context.Terms.ToListAsync();
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