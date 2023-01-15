using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.TeacherServices.Contract;
using UnitSelection.Services.TeacherServices.Contract.Dto;
using UnitSelection.Services.TeacherServices.Exceptions;
using UnitSelection.Specs.Infrastructure;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.TeacherTest.Update;

public class FailedWhenTeacherIsExist : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly TeacherService _sut;
    private Class _newClass;
    private Entities.Terms.Term _term;
    private Course _course;
    private Teacher _teacher;
    private Teacher _secondTeacher;
    private UpdateTeacherDto _dto;
    private Func<Task> _actualResult;

    public FailedWhenTeacherIsExist(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = TeacherServiceFactory.GenerateTeacherService(_context);
    }

    [BDDHelper.Given("استادی با نام ‘آرش چناری’با مدرک تحصیلی " +
                     "‘کارشناسی ارشد’ گرایش ‘مهدسی نرم افزار’" +
                     " با کد ملی ‘2294321905’ وجود دارد.")]
    [BDDHelper.And("استادی با نام ‘سعید انصاری ‘با مدرک تحصیلی" +
                   " ‘کارشناسی ‘ گرایش ‘مهدسی نرم افزار’ " +
                   "با کد ملی ‘2280509504’ وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(_term));
        _newClass = ClassFactory.GenerateClass("101", _term.Id);
        _context.Manipulate(_ => _context.Add(_newClass));
        _course = new CourseDtoBuilder().WithClassId(_newClass.Id).Build();
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
        _secondTeacher = new TeacherBuilder()
            .WithFirstName("سعید")
            .WithLastName("انصاری")
            .WithNationalCode("2294321904")
            .WithDiploma("کارشناسی ")
            .WithStudy("مهندسی کامپیوتر")
            .WithCourseId(_course.Id)
            .WithMobileNumber("91727272","98")
            .Build();
        _context.Manipulate(_ => _.Add(_secondTeacher));
    }

    [BDDHelper.When("استادی با نام ‘آرش چناری’ با مدرک تحصیلی" +
                    " ‘کارشناسی ارشد’ به گرایش ‘هوش مصنوعی’ " +
                    "با کد ملی ‘2280509504’ ویرایش می کنم.")]
    private async Task When()
    {
        _dto = new UpdateTeacherDtoBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("هوش مصنوعی")
            .WithCourseId(_course.Id)
            .Build();

        _actualResult = async () => await _sut.Update(_dto, _teacher.Id);
    }

    [BDDHelper.Then("پیغام خطایی با نام" +
                    " ‘استادی با این مشخصات در سیستم وجود دارد’ " +
                    "به کاربر نمایش میدهد.")]
    private async Task Then()
    {
        await _actualResult.Should()
            .ThrowExactlyAsync<TeacherIsExistException>();
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