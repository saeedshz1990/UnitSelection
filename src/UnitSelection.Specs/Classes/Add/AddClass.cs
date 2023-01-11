using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Classes.Contract;
using UnitSelection.Services.Classes.Contract.Dto;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.Classes.Add;

public class AddClass : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ClassService _sut;
    private AddClassDto _dto;

    public AddClass(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ClassServiceFactory.GenerateClassService(_context);
    }

    [BDDHelper.Given("هیچ کلاسی ثبت نشده است")]
    private void Given()
    {
    }

    [BDDHelper.When("یک کلاس با عنوان ‘101’ ثبت می کنم.")]
    private async Task When()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        _dto = AddClassDtoFactory
            .GenerateAddClassDto("101", term.Id);

        await _sut.Add(_dto);
    }

    [BDDHelper.Then("تنها یک کلاس با عنوان ‘101’ باید در سیستم وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult = await _context.Classes.FirstOrDefaultAsync();
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