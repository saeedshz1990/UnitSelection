using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.TermTest.Update;

public class UpdateTerm : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TermService _sut;
    private Term _term;
    private UpdateTermDto _dto;

    public UpdateTerm(ConfigurationFixture configuration) : base(configuration)
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

    [BDDHelper.When("ترم ‘مهرماه’ را به ‘ بهمن ماه’ ویرایش می کنم")]
    private async Task When()
    {
        _dto = new UpdateTermDtoBuilder()
            .WithName("بهمن ماه 1401")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();

       await _sut.Update(_term.Id, _dto);
    }

    [BDDHelper.Then("تنها یک ترم با عنوان ‘بهمن ماه’" +
                    " باید در سیستم وجود داشته باشد.")]
    private async Task Then()
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