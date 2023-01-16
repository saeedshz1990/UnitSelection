using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Entities.Terms;
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

namespace UnitSelection.Handlers.Tests.Unit.ChooseUnitHandlerTest;

public class ChooseUnitHabndlerServiceTest
{
    private readonly EFDataContext _context;
    private readonly ChooseUnitHandlerService _sut;

    public ChooseUnitHabndlerServiceTest()
    {
        _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
        _sut = HandlerServiceFactory.GenerateHandlerService(_context);
    }

    [Fact]
    public async Task Add_add_choose_unit_handler_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_=>_.Add(term));
        var newClass = new ClassBuilder()
            .WithTermId(term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_=>_.Add(newClass));
        var course = new CourseDtoBuilder()
            .WithName("شی گرایی")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(course));
        var teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithCourseId(course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(teacher));
        var student = new StudentBuilder()
            .WithFirstName("سعید")
            .WithLastName("انصاری")
            .WithNationalCode("2280509504")
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var dto = new AcceptChooseUnitDtoBuilder()
            .WithCourseId(course.Id)
            .WithStudentId(student.Id)
            .WithTeacherId(teacher.Id)
            .Build();

        await _sut.Handle(dto);
        
        var actualResult = await _context.ChooseUnits.ToListAsync();
    }
}