﻿using FluentAssertions;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
using UnitSelection.Services.Terms.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.TermTest.Add;

public class FailedWhenNameIsExist : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TermService _sut;
    private AddTermDto _dto;
    private Func<Task> _actualResult;

    public FailedWhenNameIsExist(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [BDDHelper.Given("ترمی با عنوان ترم ‘مهرماه’ وجود دارد.")]
    private void Given()
    {
        var term = new TermBuilder()
            .WithName("مهرماه 1401")
            .WithStartDate(DateTime.UtcNow)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(term));
    }

    [BDDHelper.When("یک ترم با عنوان " +
                    "‘مهرماه’ در سیستم ثبت می کنم.")]
    private async Task When()
    {
        _dto = new AddTermDtoBuilder()
            .WithName("مهرماه 1401")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();

        _actualResult = () => _sut.Add(_dto);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان ‘ نام ترم " +
                    "تکراری می باشد’ به کاربر نمایش می دهد.")]
    private async Task Then()
    {
        await _actualResult
            .Should()
            .ThrowExactlyAsync<TheTermsNameIsExistException>();
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