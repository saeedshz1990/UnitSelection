﻿using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;
using UnitSelection.Services.ChooseUnitServices.Exceptions;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.StudentTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.ChooseUnitTest.Add;

public class FailedWhenCourseUnitCountMoreThanTwenty : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ChooseUnitService _sut;
    private Class _class;
    private Class _secondClass;
    private Course _course;
    private Course _secondCourse;
    private Student _student;
    private Term _term;
    private Teacher _teacher;
    private Teacher _secondTeacher;
    private AddChooseUnitDto _dto;
    private Func<Task> _actualResult;

    public FailedWhenCourseUnitCountMoreThanTwenty(
        ConfigurationFixture configuration)
        : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ChooseUnitServiceFactory.GenerateChooseUnitServiceFactory(_context);
    }

    [BDDHelper.Given("انتخاب واحدی برای دانشجویی با نام" +
                     " ‘سعید انصاری’ با کد ملی ‘2280509504’" +
                     " با تعدا واحد ‘18’ در سیستم وجود دارد.")]
    private void Given()
    {
        _term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_ => _.Add(_term));
        _class = new ClassBuilder()
            .WithTermId(_term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_ => _.Add(_class));
        _course = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("08:00")
            .WithEndHour("11:00")
            .WithUnitCount(18)
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(_course));
        _teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithCourseId(_course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_teacher));
        _student = new StudentBuilder()
            .WithFirstName("سعید")
            .WithLastName("انصاری")
            .WithNationalCode("2280509504")
            .WithMobileNumber("9177877225", "98")
            .Build();
        _context.Manipulate(_ => _.Add(_student));
        var chooseUnit = new ChooseUnitBuilder()
            .WithClassId(_class.Id)
            .WithStudentId(_student.Id)
            .WithTeacherId(_teacher.Id)
            .WithTermId(_term.Id)
            .WithCourseId(_course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(chooseUnit));
    }

    [BDDHelper.When("کلاس ‘101’ و درس ‘شی گرایی’ " +
                    "با استاد ‘ارش چناری’" +
                    " با تعداد واحد ‘3’ انتخاب می کنم.")]
    private async Task When()
    {
        _secondCourse = new CourseDtoBuilder()
            .WithName(" شی گرایی ")
            .WithStartHour("11:00")
            .WithEndHour("13:00")
            .WithUnitCount(3)
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(_secondCourse));

        _dto = new AddChooseUnitDto
        {
            StudentId = _student.Id,
            CourseId = _secondCourse.Id,
            TermId = _term.Id,
            TeacherId = _teacher.Id,
            ClassId = _class.Id
        };

        _actualResult = async () => await _sut.Add(_dto);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان " +
                    "‘ تعداد واحدهای انتخابی بیشتر از سقف مجاز می باشد ‘" +
                    " به کاربر  نمایش می دهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<CountOfCourseUnitMoreThanTwentyException>();
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