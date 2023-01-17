using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Students;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Contracts.Dto;
using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.Persistence.EF.StudentPersistence;

public class EFStudentRepository : StudentRepository
{
    private readonly DbSet<Student> _students;
    private readonly DbSet<ChooseUnit> _chooseUnits;

    public EFStudentRepository(EFDataContext context)
    {
        _students = context.Set<Student>();
        _chooseUnits = context.Set<ChooseUnit>();
    }

    public void Add(Student student)
    {
        _students.Add(student);
    }

    public void Update(Student dto)
    {
        _students.Update(dto);
    }

    public IList<GetStudentDto> GetAll()
    {
        return _students.Select(_ => new GetStudentDto
        {
            Id = _.Id,
            FirstName = _.FirstName,
            LastName = _.LastName,
            FatherName = _.FatherName,
            NationalCode = _.NationalCode,
            DateOfBirth = _.DateOfBirth,
            Address = _.Address,
            Mobile = new GetMobileDto(
                _.Mobile.CountryCallingCode,
                _.Mobile.MobileNumber)
        }).ToList();
    }

    public GetStudentByIdDto GetById(int id)
    {
        return _students
            .Where(_ => _.Id == id)
            .Select(_ => new GetStudentByIdDto
            {
                FirstName = _.FirstName,
                LastName = _.LastName,
                FatherName = _.FatherName,
                NationalCode = _.NationalCode,
                DateOfBirth = _.DateOfBirth,
                Address = _.Address,
                Mobile = new GetMobileDto(
                    _.Mobile.CountryCallingCode,
                    _.Mobile.MobileNumber)
            }).FirstOrDefault()!;
    }

    public void Delete(Student student)
    {
        _students.Remove(student);
    }

    public Student? FindById(int id)
    {
        return _students.FirstOrDefault(_ => _.Id == id);
    }

    public bool IsExistChooseUnit(int studentId)
    {
        return _chooseUnits.Any(_ => _.StudentId == studentId);
    }

    public bool IsExistNationalCode(string nationalCode)
    {
        return _students
            .Any(_ => _.NationalCode == nationalCode);
    }

    public bool IsStudentExist(int id)
    {
        return _students.Any(_ => _.Id == id);
    }
}