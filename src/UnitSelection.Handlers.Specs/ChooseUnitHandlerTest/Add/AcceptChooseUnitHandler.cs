using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Entities.Terms;
using UnitSelection.Handlers.Specs.Infrastructure;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts.Dto;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.HandlerTestTools.AcceptChooseUnitHandler;
using UnitSelection.TestTools.StudentTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Handlers.Specs.ChooseUnitHandlerTest.Add;

public class AcceptChooseUnitHandler : EFDataContextDatabaseFixture
{
    private readonly EFDataContext _context;
    private readonly ChooseUnitHandlerService _sut;
    private Class _class;
    private Course _course;
    private Student _student;
    private Term _term;
    private Teacher _teacher;
    private AcceptChooseUnitDto _dto;

    public AcceptChooseUnitHandler(
        ConfigurationFixture configuration)
        : base(configuration)
    {
        _context = CreateDataContext();
        _sut = HandlerServiceFactory.GenerateHandlerService(_context);
    }

    [BDDHelper.Given("هیچ انتخاب واحدی برای" +
                     " دانشجویی با نام ‘سعید انصاری’" +
                     " با کد ملی ‘2280509504’ در" +
                     " سیستم وجود ندارد.")]
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
            .WithClassId(_class.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(_course));
        _teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithMobileNumber("933321212", "98")
            .WithCourseId(_course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(_teacher));
        _student = new StudentBuilder()
            .WithFirstName("سعید")
            .WithLastName("انصاری")
            .WithNationalCode("2280509504")
            .WithMobileNumber("936789123", "98")
            .Build();
        _context.Manipulate(_ => _.Add(_student));
    }

    [BDDHelper.When("کلاس ‘101’ و درس ‘شی گرایی’ با استاد" +
                    " ‘ارش چناری’ انتخاب می کنم.")]
    private async Task When()
    {
        _dto = new AcceptChooseUnitDtoBuilder()
            .WithCourseId(_course.Id)
            .WithStudentId(_student.Id)
            .WithTeacherId(_teacher.Id)
            .Build();

        await _sut.Handle(_dto);
    }

    [BDDHelper.Then("تنها یک انتخاب واحدی با عنوان درس" +
                    " ‘شی گرایی’ با تعداد واحد ‘3’" +
                    " و کلاس ‘101’ با استاد ‘آرش چناری’" +
                    " در لیست انتخاب واحد دانشجوی " +
                    "‘سعید انصاری’ باید وجود داشته باشد.")]
    private async Task Then()
    {
        var actualResult = await _context.ChooseUnits.ToListAsync();
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