using FluentAssertions;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.ClassServices.Contract;
using UnitSelection.Services.ClassServices.Contract.Dto;
using UnitSelection.Services.ClassServices.Exceptions;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.ClassTest.Add;

public class FailedWhenClassNameIsDuplicated : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ClassService _sut;
    private AddClassDto _dto;
    private Term _term;
    private Func<Task> _actualResult;

    public FailedWhenClassNameIsDuplicated(
        ConfigurationFixture configuration)
        : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ClassServiceFactory.GenerateClassService(_context);
    }

    [BDDHelper.Given("کلاسی با عنوان ‘101’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(_term));
        var newClass = ClassFactory.GenerateClass("101", _term.Id);
        _context.Manipulate(_ => _.Add(newClass));
    }

    [BDDHelper.When("یک کلاس با عنوان ‘101’ ثبت می کنم.")]
    private async Task When()
    {

        _dto = AddClassDtoFactory
            .GenerateAddClassDto("101", _term.Id);

        _actualResult = async () => await _sut.Add(_dto);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان ‘" +
                    "نام کلاس تکراری می باشد’" +
                    " به کاربر نمایش می دهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<ClassNameIsDuplicatedException>();
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