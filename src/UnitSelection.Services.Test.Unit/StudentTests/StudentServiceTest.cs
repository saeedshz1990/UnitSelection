using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Exceptions;
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
        
       await actualResult.Should().
            ThrowExactlyAsync<StudentIsExistException>();
    }
}