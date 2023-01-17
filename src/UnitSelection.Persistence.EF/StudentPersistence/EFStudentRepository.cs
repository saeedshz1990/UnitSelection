using UnitSelection.Entities.Students;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Contracts.Dto;
using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.Persistence.EF.StudentPersistence;

public class EFStudentRepository : StudentRepository
{
    private readonly EFDataContext _context;

    public EFStudentRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(Student student)
    {
        _context.Add(student);
    }

    public void Update(Student dto)
    {
        _context.Update(dto);
    }

    public IList<GetStudentDto> GetAll()
    {
        return _context.Students.Select(_ => new GetStudentDto
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
        return _context.Students
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
        _context.Remove(student);
    }

    public Student? FindById(int id)
    {
        return _context.Students.FirstOrDefault(_ => _.Id == id);
    }

    public bool IsExistChooseUnit(int studentId)
    {
        return _context.ChooseUnits.Any(_ => _.StudentId == studentId);
    }

    public bool IsExistNationalCode(string nationalCode)
    {
        return _context.Students
            .Any(_ => _.NationalCode == nationalCode);
    }

    public bool IsStudentExist(int id)
    {
        return _context.Students.Any(_ => _.Id == id);
    }
}