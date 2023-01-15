using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.ClassServices.Contract;
using UnitSelection.Services.ClassServices.Contract.Dto;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.ClassTest.Update;

public class UpdateClass : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ClassService _sut;
    private UpdateClassDto _dto;
    private Entities.Terms.Term _term;
    private Class _firstClass;


    public UpdateClass(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ClassServiceFactory.GenerateClassService(_context);
    }

    [BDDHelper.Given("کلاس با عنوان ‘102’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _.Add(_term));
        _firstClass = ClassFactory.GenerateClass("102", _term.Id);
        _context.Manipulate(_ => _.Add(_firstClass));
    }

    [BDDHelper.When("کلاس ‘102’ را به ‘101’ ویرایش می کنم")]
    private async Task When()
    {
        _dto = UpdateClassDtoFactory
            .GenerateUpdateClassDto("101", _term.Id);

       await _sut.Update(_dto, _firstClass.Id);
    }

    [BDDHelper.Then("تنها یک کلاس با عنوان ‘101’ باید وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult = await _context.Classes.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(_dto.Name);
        actualResult.TermId.Should().Be(_dto.TermId);
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