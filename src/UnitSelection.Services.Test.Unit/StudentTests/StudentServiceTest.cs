using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Exceptions;
using UnitSelection.TestTools.ChooseUnitTestTools;
using UnitSelection.TestTools.StudentTestTools;
using Xunit;

namespace UnitSelection.Services.Test.Unit.StudentTests;

public class StudentServiceTest
{
    private readonly EFDataContext _context;
    private readonly StudentService _sut;

    public StudentServiceTest()
    {
        _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
        _sut = StudentServiceFactory.GenerateStudentService(_context);
    }

    [Fact]
    public async Task Add_add_student_properly()
    {
        var dto = new AddStudentDtoBuilder()
            .Build();

        await _sut.Add(dto);

        var actualResult = await _context.Students.FirstOrDefaultAsync();
        actualResult!.FirstName.Should().Be(dto.FirstName);
        actualResult.LastName.Should().Be(dto.LastName);
        actualResult.FatherName.Should().Be(dto.FatherName);
        actualResult.Address.Should().Be(dto.Address);
        actualResult.NationalCode.Should().Be(dto.NationalCode);
        actualResult.DateOfBirth.Should().Be(dto.DateOfBirth);
        actualResult.Mobile.MobileNumber.Should().Be(dto.Mobile.MobileNumber);
        actualResult.Mobile.CountryCallingCode.Should().Be(dto.Mobile.CountryCallingCode);
    }

    [Fact]
    public async Task Add_throw_exception_when_student_is_exist_properly()
    {
        var student = new StudentBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var dto = new AddStudentDtoBuilder()
            .Build();

        var actualResult = async () => await _sut.Add(dto);

        await actualResult.Should().ThrowExactlyAsync<StudentIsExistException>();
    }

    [Fact]
    public async Task Update_update_student_propely()
    {
        var student = new StudentBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var dto = new UpdateStudentDtoBuilder()
            .WithFirstName("updatedDummy")
            .WithLastName("updatedLast")
            .WithMobileNumber("updatedMob", "98")
            .Build();

        await _sut.Update(dto, student.Id);

        var actualResult = await _context.Students
            .FirstOrDefaultAsync(_ => _.Id == student.Id);
        actualResult!.FirstName.Should().Be(dto.FirstName);
        actualResult.LastName.Should().Be(dto.LastName);
        actualResult.FatherName.Should().Be(dto.FatherName);
        actualResult.Address.Should().Be(dto.Address);
        actualResult.DateOfBirth.Should().Be(dto.DateOfBirth);
        actualResult.Mobile.MobileNumber.Should().Be(dto.Mobile.MobileNumber);
        actualResult.Mobile.CountryCallingCode.Should().Be(dto.Mobile.CountryCallingCode);
    }

    [Theory]
    [InlineData(-1)]
    public async Task Update_throw_exception_when_student_not_exist_properly(int invalidId)
    {
        var dto = new UpdateStudentDtoBuilder()
            .WithFirstName("محمدرضا")
            .WithLastName("انصاری")
            .WithFatherName("محمدجواد")
            .WithDateOfBirth("1369")
            .WithMobileNumber("9177877225", "98")
            .Build();

        var actualResult = async () => await _sut.Update(dto, invalidId);

        await actualResult.Should().ThrowExactlyAsync<StudentNotFoundException>();
    }

    [Fact]
    public async Task Update_throw_exception_when_student_is_exist_properly()
    {
        var student = new StudentBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var secondStudent = new StudentBuilder()
            .WithFirstName("secondDummy")
            .WithLastName("secondLast")
            .WithNationalCode("secondCode")
            .WithMobileNumber("secondMob", "98")
            .Build();
        _context.Manipulate(_ => _.Add(secondStudent));
        var dto = new UpdateStudentDtoBuilder()
            .WithFirstName("secondDummy")
            .WithLastName("secondLast")
            .WithMobileNumber("secondMob", "98")
            .Build();

        var actualResult = async () => await _sut.Update(dto, student.Id);

        await actualResult.Should().ThrowExactlyAsync<StudentIsExistException>();
    }

    [Fact]
    public async Task Get_get_all_student_properly()
    {
        var student = new StudentBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var secondStudent = new StudentBuilder()
            .WithFirstName("secondDummy")
            .WithLastName("secondLast")
            .WithNationalCode("secondCode")
            .WithMobileNumber("secondMob", "98")
            .Build();
        _context.Manipulate(_ => _.Add(secondStudent));

        _sut.GetAll();

        var actualResult = await _context.Students.ToListAsync();
        actualResult.Should().HaveCount(2);
    }

    [Fact]
    public async Task Get_get_all_not_any_student_exist_properly()
    {
        _sut.GetAll();

        var actualResult = await _context.Students.ToListAsync();
        actualResult.Should().HaveCount(0);
    }

    [Fact]
    public async Task Get_get_by_id_student_properly()
    {
        var student = new StudentBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var secondStudent = new StudentBuilder()
            .WithFirstName("secondDummy")
            .WithLastName("secondLast")
            .WithNationalCode("secondCode")
            .WithMobileNumber("secondMob", "98")
            .Build();
        _context.Manipulate(_ => _.Add(secondStudent));

        _sut.GetById(student.Id);

        var actualResult = await _context.Students
            .FirstOrDefaultAsync(_ => _.Id == student.Id);

        actualResult!.FirstName.Should().Be(student.FirstName);
        actualResult.LastName.Should().Be(student.LastName);
        actualResult.FatherName.Should().Be(student.FatherName);
        actualResult.Address.Should().Be(student.Address);
        actualResult.NationalCode.Should().Be(student.NationalCode);
        actualResult.DateOfBirth.Should().Be(student.DateOfBirth);
        actualResult.Mobile.MobileNumber.Should().Be(student.Mobile.MobileNumber);
        actualResult.Mobile.CountryCallingCode.Should().Be(student.Mobile.CountryCallingCode);
    }

    [Fact]
    public async Task Delete_delete_student_properly()
    {
        var student = new StudentBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(student));

        await _sut.Delete(student.Id);

        var actualResult = await _context.Students.ToListAsync();
        actualResult.Should().HaveCount(0);
    }

    [Theory]
    [InlineData(-1)]
    public async Task Delete_throw_exception_when_student_not_found_properly(int invalidId)
    {
        var actualResult = async () => await _sut.Delete(invalidId);

        await actualResult.Should().ThrowExactlyAsync<StudentNotFoundException>();
    }

    [Fact]
    public async Task Delete_throw_exception_when_student_have_choose_unit_properly()
    {
        var student = new StudentBuilder()
            .Build();
        _context.Manipulate(_ => _.Add(student));
        var chooseUnit = new ChooseUnitBuilder()
            .WithStudentId(student.Id)
            .Build();
        _context.Manipulate(_ => _.Add(chooseUnit));

        var actualResult = async () => await _sut.Delete(student.Id);

        await actualResult.Should()
            .ThrowExactlyAsync<StudentHaveChooseUnitException>();
    }
}