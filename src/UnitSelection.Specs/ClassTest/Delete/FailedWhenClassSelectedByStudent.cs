using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.ClassServices.Contract;
using UnitSelection.Services.ClassServices.Exceptions;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.ClassTest.Delete;

public class FailedWhenClassSelectedByStudent : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ClassService _sut;
    private Entities.Terms.Term _term;
    private Class _firstClass;
    private Func<Task> _actualResult;

    public FailedWhenClassSelectedByStudent(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ClassServiceFactory.GenerateClassService(_context);
    }

    [BDDHelper.Given("کلاس با عنوان ‘101’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _.Add(_term));
        _firstClass = new ClassBuilder()
            .WithName("101")
            .WithTermId(_term.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_firstClass));
        var chooseUnit = new ChooseUnitBuilder()
            .WithClassId(_firstClass.Id)
            .Build();
        _context.Manipulate(_ => _.Add(chooseUnit));
    }

    [BDDHelper.When("کلاس را حذف می کنم.")]
    private async Task When()
    {
        _actualResult = async () => await _sut.Delete(_firstClass.Id);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان " +
                    "‘کلاس انتخاب شده توسط دانشجو نمی تواند حذف شود’" +
                    " به کاربر  نمایش می دهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<ClassSelectedByStudentException>();
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