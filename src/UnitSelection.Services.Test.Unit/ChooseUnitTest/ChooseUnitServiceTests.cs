using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;
using UnitSelection.Services.ChooseUnitServices.Exceptions;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Exceptions;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.CourseTestTools;
using UnitSelection.TestTools.StudentTestTools;
using UnitSelection.TestTools.TeacherTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Services.Test.Unit.ChooseUnitTest;

public class ChooseUnitServiceTests
{
    private readonly EFDataContext _context;
    private readonly ChooseUnitService _sut;

    public ChooseUnitServiceTests()
    {
        _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
        _sut = ChooseUnitServiceFactory.GenerateChooseUnitServiceFactory(_context);
    }

    [Fact]
    public async Task Add_add_choose_unit_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_ => _.Add(term));
        var newClass = new ClassBuilder()
            .WithTermId(term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("08:00")
            .WithEndHour("11:00")
            .WithUnitCount(18)
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
            .WithMobileNumber("9177877225", "98")
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var dto = new AddChooseUnitDto
        {
            StudentId = student.Id,
            CourseId = course.Id,
            TermId = term.Id,
            TeacherId = teacher.Id,
            ClassId = newClass.Id
        };

        await _sut.Add(dto);

        var actual = _context.ChooseUnits.ToList();
        actual.Should().HaveCount(1);
    }

    [Fact]
    public async Task Add_throw_exception_when_UnitCount_more_than_twenty_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_ => _.Add(term));
        var newClass = new ClassBuilder()
            .WithTermId(term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("08:00")
            .WithEndHour("11:00")
            .WithUnitCount(18)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(course));
        var secondCourse = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("12:00")
            .WithEndHour("14:00")
            .WithUnitCount(3)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(secondCourse));
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
            .WithMobileNumber("9177877225", "98")
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var chooseUnit = new ChooseUnitBuilder()
            .WithClassId(newClass.Id)
            .WithStudentId(student.Id)
            .WithTeacherId(teacher.Id)
            .WithTermId(term.Id)
            .WithCourseId(course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(chooseUnit));
        var dto = new AddChooseUnitDto
        {
            StudentId = student.Id,
            CourseId = secondCourse.Id,
            TermId = term.Id,
            TeacherId = teacher.Id,
            ClassId = newClass.Id
        };

        var actualResult = async () => await _sut.Add(dto);

        await actualResult.Should()
            .ThrowExactlyAsync<CountOfCourseUnitMoreThanTwentyException>();
    }

    [Fact]
    public async Task Add_throw_Exception_when_conflicted_course_hour_properly()
    {
        var term = new TermBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(term));
        var newClass = new ClassBuilder()
            .WithTermId(term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder()
            .WithName("شی گرایی")
            .WithStartHour("08:00")
            .WithEndHour("11:00")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(course));
        var secondCourse = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("09:00")
            .WithEndHour("10:45")
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(secondCourse));
        var teacher = new TeacherBuilder()
            .WithFirstName("آرش")
            .WithLastName("چناری")
            .WithNationalCode("2294321905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithCourseId(course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(teacher));
        var newTeacher = new TeacherBuilder()
            .WithFirstName("رضا")
            .WithLastName("محمدیان")
            .WithNationalCode("2294328905")
            .WithDiploma("کارشناسی ارشد")
            .WithStudy("مهندسی نرم افزار")
            .WithCourseId(secondCourse.Id)
            .Build();
        _context.Manipulate(_ => _.Add(newTeacher));
        var student = new StudentBuilder()
            .WithFirstName("سعید")
            .WithLastName("انصاری")
            .WithNationalCode("2280509504")
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var chooseUnit = new ChooseUnitBuilder()
            .WithClassId(newClass.Id)
            .WithStudentId(student.Id)
            .WithTeacherId(teacher.Id)
            .WithTermId(term.Id)
            .WithCourseId(course.Id)
            .Build();
        _context.Manipulate(_ => _.Add(chooseUnit));
        var dto = new AddChooseUnitDtoBuilder()
            .WithClassId(newClass.Id)
            .WithStudentId(student.Id)
            .WithTeacherId(newTeacher.Id)
            .WithTermId(term.Id)
            .WithCourseId(secondCourse.Id)
            .Build();

        var actualResult = async () => await _sut.Add(dto);
        
        await actualResult.Should()
            .ThrowExactlyAsync<ConflictingCourseHourException>();
        
    }

    [Fact]
    public async Task Get_get_all_chooseUnit_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_ => _.Add(term));
        var newClass = new ClassBuilder()
            .WithTermId(term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("08:00")
            .WithEndHour("11:00")
            .WithUnitCount(18)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(course));
        var secondCourse = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("12:00")
            .WithEndHour("14:00")
            .WithUnitCount(3)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(secondCourse));
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
            .WithMobileNumber("9177877225", "98")
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var dto = new ChooseUnitBuilder()
            .WithClassId(newClass.Id)
            .WithCourseId(course.Id)
            .WithStudentId(student.Id)
            .WithTeacherId(teacher.Id)
            .WithTermId(term.Id)
            .Build();
        _context.Manipulate(_ => _.Add(dto));

        _sut.GetAll();

        var actualResult = _context.ChooseUnits.ToList();
        actualResult.Should().HaveCount(1);
    }

    [Fact]
    public async Task Get_get_by_id_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_ => _.Add(term));
        var newClass = new ClassBuilder()
            .WithTermId(term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("08:00")
            .WithEndHour("11:00")
            .WithUnitCount(18)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(course));
        var secondCourse = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("12:00")
            .WithEndHour("14:00")
            .WithUnitCount(3)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(secondCourse));
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
            .WithMobileNumber("9177877225", "98")
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var dto = new ChooseUnitBuilder()
            .WithClassId(newClass.Id)
            .WithCourseId(course.Id)
            .WithStudentId(student.Id)
            .WithTeacherId(teacher.Id)
            .WithTermId(term.Id)
            .Build();
        _context.Manipulate(_ => _.Add(dto));

        _sut.GetById(dto.Id);

        var actualResult = _context.ChooseUnits.FirstOrDefault();
        actualResult!.ClassId.Should().Be(dto.ClassId);
        actualResult.CourseId.Should().Be(dto.CourseId);
        actualResult.StudentId.Should().Be(dto.StudentId);
        actualResult.TeacherId.Should().Be(dto.TeacherId);
        actualResult.TermId.Should().Be(dto.TermId);
    }

    [Fact]
    public async Task Get_get_by_term_id_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_ => _.Add(term));
        var secondTerm = TermServiceFactoryDto.GenerateTerms("secondTerm");
        _context.Manipulate(_ => _.Add(secondTerm));
        var newClass = new ClassBuilder()
            .WithTermId(term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("08:00")
            .WithEndHour("11:00")
            .WithUnitCount(18)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(course));
        var secondCourse = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("12:00")
            .WithEndHour("14:00")
            .WithUnitCount(3)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(secondCourse));
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
            .WithMobileNumber("9177877225", "98")
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var dto = new ChooseUnitBuilder()
            .WithClassId(newClass.Id)
            .WithCourseId(course.Id)
            .WithStudentId(student.Id)
            .WithTeacherId(teacher.Id)
            .WithTermId(term.Id)
            .Build();
        _context.Manipulate(_ => _.Add(dto));

        _sut.GetByTermId(dto.TermId);

        var actualResult = _context.ChooseUnits.FirstOrDefault(_ => _.TermId == dto.TermId);
        actualResult!.ClassId.Should().Be(dto.ClassId);
        actualResult.CourseId.Should().Be(dto.CourseId);
        actualResult.StudentId.Should().Be(dto.StudentId);
        actualResult.TeacherId.Should().Be(dto.TeacherId);
    }

    [Fact]
    public async Task Delete_delete_choose_unit_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_ => _.Add(term));
        var secondTerm = TermServiceFactoryDto.GenerateTerms("secondTerm");
        _context.Manipulate(_ => _.Add(secondTerm));
        var newClass = new ClassBuilder()
            .WithTermId(term.Id)
            .WithName("101")
            .Build();
        _context.Manipulate(_ => _.Add(newClass));
        var course = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("08:00")
            .WithEndHour("11:00")
            .WithUnitCount(18)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(course));
        var secondCourse = new CourseDtoBuilder()
            .WithName("مهندسی نرم افزار")
            .WithStartHour("12:00")
            .WithEndHour("14:00")
            .WithUnitCount(3)
            .WithClassId(newClass.Id)
            .Build();
        _context.Manipulate(_ => _context.Add(secondCourse));
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
            .WithMobileNumber("9177877225", "98")
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var dto = new ChooseUnitBuilder()
            .WithClassId(newClass.Id)
            .WithCourseId(course.Id)
            .WithStudentId(student.Id)
            .WithTeacherId(teacher.Id)
            .WithTermId(term.Id)
            .Build();
        _context.Manipulate(_ => _.Add(dto));

        await _sut.Delete(dto.Id);

        var actualResult = await _context.ChooseUnits.ToListAsync();
        actualResult.Should().HaveCount(0);
    }

    [Theory]
    [InlineData(-1)]
    public async Task Delete_throw_exception_when_not_found_choose_unit_properly(int invalidId)
    {
        var actualResult = async () => await _sut.Delete(invalidId);

        await actualResult.Should()
            .ThrowExactlyAsync<ChooseUnitNotFoundException>();
    }
}