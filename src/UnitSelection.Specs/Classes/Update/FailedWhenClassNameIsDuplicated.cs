using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Classes.Contract;
using UnitSelection.Services.Classes.Contract.Dto;
using UnitSelection.Services.Classes.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.Classes.Update;

public class FailedWhenClassNameIsDuplicated : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ClassService _sut;
    private Class _firstClass;
    private Class _secondClass;
    private Entities.Terms.Term _term;
    private UpdateClassDto _dto;
    private Func<Task> _actualResult;

    public FailedWhenClassNameIsDuplicated(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ClassServiceFactory.GenerateClassService(_context);
    }

    [BDDHelper.Given("کلاس با عنوان ‘102’ وجود دارد.")]
    [BDDHelper.And("کلاسی با عنوان ‘101’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _.Add(_term));
        _firstClass = ClassFactory.GenerateClass("102", _term.Id);
        _context.Manipulate(_ => _.Add(_firstClass));
        _secondClass = ClassFactory.GenerateClass("101", _term.Id);
        _context.Manipulate(_ => _.Add(_secondClass));
    }

    [BDDHelper.When("کلاس ‘102’ را به ‘101’ ویرایش می کنم")]
    private async Task When()
    {
        _dto = UpdateClassDtoFactory
            .GenerateUpdateClassDto("101", _term.Id);

        _actualResult = async () => await _sut.Update(_dto, _firstClass.Id);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان" +
                    " ‘نام کلاس تکراری می باشد’" +
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