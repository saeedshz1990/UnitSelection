using FluentAssertions;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Infrastructures.Test;
using UnitSelection.Infrastructures.Test.Infrastructure;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.StudentTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Specs.ChooseUnitTest.Add;

public class AddChooseUnit : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ChooseUnitService _sut;
    private Class _class;
    private Class _secondClass;
    private Course _course;
    private Course _secondCourse;
    private Student _student;
    private Entities.Terms.Term _term;
    private Teacher _teacher;
    private Teacher _secondTeacher;
    private AddChooseUnitDto _dto;

    public AddChooseUnit(ConfigurationFixture configuration) : base(configuration)
    {
        _context = CreateDataContext();
        _sut = ChooseUnitServiceFactory.GenerateChooseUnitServiceFactory(_context);
    }
    
    [BDDHelper.Given("")]
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
    }

    [BDDHelper.When("")]
    private async Task When()
    {
        _dto = new AddChooseUnitDto
        {
            StudentId = _student.Id,
            CourseId = _course.Id,
            TermId = _term.Id,
            TeacherId = _teacher.Id,
            ClassId = _class.Id
        };

         await _sut.Add(_dto);
    }

    [BDDHelper.Then("")]
    private async Task Then()
    {
        var actual = _context.ChooseUnits.ToList();
        actual.Should().HaveCount(1);
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