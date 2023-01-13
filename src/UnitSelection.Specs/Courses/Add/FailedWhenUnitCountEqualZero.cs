﻿using FluentAssertions;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Courses.Contract;
using UnitSelection.Services.Courses.Contract.Dto;
using UnitSelection.Services.Courses.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TermTestTools;

namespace UnitSelection.Specs.Courses.Add;

public class FailedWhenUnitCountEqualZero :EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly CourseService _sut;
    private AddCourseDto _dto;
    private Func<Task> _actualResult;
    
    public FailedWhenUnitCountEqualZero(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = CourseServiceFactory.GenerateCourseService(_context);
    }
    
    [BDDHelper.Given("هیچ درسی در سیستم ثبت نشده است")]
    private void Given()
    {
    }

    [BDDHelper.When("یک درس با عنوان ‘ریاضی مهندسی’  و تعداد واجد صفردر سیستم ثبت می کنم.")]
    private async Task When()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = ClassFactory.GenerateClass("101", term.Id);
        _context.Manipulate(_ => _context.Add(newClass));

        _dto = new AddCourseDtoBuilder()
            .WithName("ریاضی مهندسی")
            .WithDayOfWeek("یکشنبه")
            .WithUnitCount(0)
            .WithStartHour("10:00")
            .WithEndHour("13:00")
            .WithClassId(newClass.Id)
            .Build();

        _actualResult = async () => await _sut.Add(_dto);    }

    [BDDHelper.Then("پیغام خطایی با عنوان ' تعداد واحد نم یتواند صفر باشد' به کاربر نمایش می دهد")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<CourseUnitCountCanNotBeZeroException>();
    }
}