using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.StudentTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.ChooseUnitTest.Delete;

public class DeleteChooseUnit :EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ChooseUnitService _sut;
    private Class _class;
    private Course _course;
    private Student _student;
    private Entities.Terms.Term _term;
    private Teacher _teacher;
    private ChooseUnit _chooseUnit;
    
    public DeleteChooseUnit(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ChooseUnitServiceFactory.GenerateChooseUnitServiceFactory(_context);
    }
    
    [BDDHelper.Given("انتخاب واحدی برای دانشجویی با نام" +
                     " ‘سعید انصاری’ با کد ملی ‘2280509504’" +
                     " یک درس با عنوان ‘شی گرایی’" +
                     " با استاد ‘آرش چناری’ و کلاس ‘101’" +
                     " و ساعت 8:00 تا 11:00 وجود دارد.")]
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
        _chooseUnit = new ChooseUnitBuilder()
            .WithTermId(_term.Id)
            .WithClassId(_class.Id)
            .WithCourseId(_course.Id)
            .WithStudentId(_student.Id)
            .WithTeacherId(_teacher.Id)
            .Build();
        _context.Manipulate(_=>_.Add(_chooseUnit));
    }

    [BDDHelper.When("انتخاب واحد دانشجویی با نام ‘سعید انصاری’ کلاس ‘101’" +
                    " و درس ‘شی گرایی’ را حذف می کنم.")]
    private async Task When()
    {
        await _sut.Delete(_chooseUnit.Id);
    }

    [BDDHelper.Then("هیچ انتخاب واحدی برای دانشجویی با نام" +
                    " ‘ سعید انصاری’ در سیستم وجود ندارد.")]
    private async Task Then()
    {
        var actualResult = await _context.ChooseUnits.ToListAsync();
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