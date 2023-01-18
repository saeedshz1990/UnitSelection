using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;
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

public class FailedWhenConflictedCourseHour : EFDataContextDatabaseFixture
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
    private Teacher _newTeacher;
    private AddChooseUnitDto _dto;
    private Func<Task> _actualResult;

    public FailedWhenConflictedCourseHour(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ChooseUnitServiceFactory.GenerateChooseUnitServiceFactory(_context);
    }

    [BDDHelper.Given("انتخاب واحدی برای دانشجویی با نام ‘سعید انصاری’" +
                     " با کد ملی ‘2280509504’ یک درس با عنوان" +
                     " ‘شی گرایی’ با استاد ‘آرش چناری’ و کلاس ‘101’" +
                     " و ساعت 8:00 تا 11:00 انتخاب شده وجود دارد.")]
    private void Given()
    {
        _term = new TermBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(_term));
        _class = new ClassBuilder()
            .WithTermId(_term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_ => _.Add(_class));
        _course = new CourseDtoBuilder()
            .WithName("شی گرایی")
            .WithStartHour("08:00")
            .WithEndHour("11:00")
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(_course));
         _secondCourse = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("09:00")
            .WithEndHour("10:45")
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(_secondCourse));
        _teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithCourseId(_course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_teacher));
        _newTeacher = new TeacherBuilder()
            .WithFirstName("رضا")
            .WithLastName("محمدیان")
            .WithNationalCode("2294328905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithMobileNumber("91772222","98")
            .WithCourseId(_secondCourse.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_newTeacher));
        _student = new StudentBuilder()
            .WithFirstName("سعید")
            .WithLastName("انصاری")
            .WithNationalCode("2280509504")
            .WithMobileNumber("newdummy","98")
            .Build();
        _context.Manipulate(_ => _.Add(_student));
    }

    [BDDHelper.When("کلاس ‘101’ و درس ‘مهندسی نرم افزار’ با استاد " +
                    "‘محمد محمدیانی’ با انتخاب کلاس ‘101’ " +
                    "و ساعت 9:00 تا 10:45 انتخاب می کنم.")]
    private async Task When()
    {
        var chooseUnit = new ChooseUnitBuilder()
            .WithClassId(_class.Id)
            .WithStudentId(_student.Id)
            .WithTeacherId(_teacher.Id)
            .WithTermId(_term.Id)
            .WithCourseId(_course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(chooseUnit));
        _dto = new AddChooseUnitDtoBuilder()
            .WithClassId(_class.Id)
            .WithStudentId(_student.Id)
            .WithTeacherId(_newTeacher.Id)
            .WithTermId(_term.Id)
            .WithCourseId(_secondCourse.Id)
            .Build();

        _actualResult = async () => await _sut.Add(_dto);
    }

    [BDDHelper.Then("پیغام خطایی با عنوان" +
                    " ‘بدلیل تداخل زمانی انتخاب درس امکان پذیر نمی باشد’" +
                    " به کاربر نمایش می دهد.")]
    private async Task Then()
    {
        await _actualResult.Should().ThrowExactlyAsync<ConflictingCourseHourException>();
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