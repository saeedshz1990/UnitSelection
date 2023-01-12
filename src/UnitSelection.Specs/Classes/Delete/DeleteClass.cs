﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Classes.Contract;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.Classes.Delete;

public class DeleteClass: EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ClassService _sut;
    private Entities.Terms.Term _term;
    private Class _firstClass;
    
    public DeleteClass(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ClassServiceFactory.GenerateClassService(_context);
    }
    
    [BDDHelper.Given("کلاس با عنوان ‘101’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _.Add(_term));
        _firstClass = ClassFactory.GenerateClass("101", _term.Id);
        _context.Manipulate(_ => _.Add(_firstClass));
    }

    [BDDHelper.When("کلاس را حذف می کنم.")]
    private async Task When()
    {
        await _sut.Delete(_firstClass.Id);
    }

    [BDDHelper.Then("نباید هیچ کلاسی در سیستم وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult =await _context.Classes.ToListAsync();
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