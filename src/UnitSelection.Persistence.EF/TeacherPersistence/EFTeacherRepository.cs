using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities;
using UnitSelection.Entities.Teachers;
using UnitSelection.Services.TeacherServices.Contract;
using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.Persistence.EF.TeacherPersistence;

public class EFTeacherRepository : TeacherRepository
{
    private readonly EFDataContext _context;

    public EFTeacherRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(Teacher teacher)
    {
        _context.Add(teacher);
    }

    public IList<GetTeacherDto> GetAll()
    {
        return _context.Teachers.Select(_ => new GetTeacherDto
        {
            Id = _.Id,
            FirstName = _.FirstName,
            LastName = _.LastName,
            FatherName = _.FatherName,
            NationalCode = _.NationalCode,
            Address = _.Address,
            Study = _.Study,
            Diploma = _.Diploma,
            DateOfBirth = _.DateOfBirth,
            GroupOfCourse = _.GroupOfCourse,
            Mobile = new GetMobileDto(_.Mobile.CountryCallingCode, _.Mobile.MobileNumber),
            CourseId = _.CourseId
        }).ToList();
    }

    public GetTeacherByIdDto GetById(int id)
    {
        return _context.Teachers
            .Where(_ => _.Id == id)
            .Select(_ => new GetTeacherByIdDto
            {
                FirstName = _.FirstName,
                LastName = _.LastName,
                FatherName = _.FatherName,
                NationalCode = _.NationalCode,
                Address = _.Address,
                Study = _.Study,
                Diploma = _.Diploma,
                DateOfBirth = _.DateOfBirth,
                GroupOfCourse = _.GroupOfCourse,
                Mobile = new GetMobileDto(_.Mobile.CountryCallingCode, _.Mobile.MobileNumber),
                CourseId = _.CourseId
            }).FirstOrDefault()!;
    }

    public GetTeacherByCourseIdDto GetTeacherByCourseId(int courseId)
    {
        return _context.Teachers
            .Where(_=>_.CourseId==courseId)
            .Select(_ => new GetTeacherByCourseIdDto()
            {
                FirstName = _.FirstName,
                LastName = _.LastName,
                FatherName = _.FatherName,
                NationalCode = _.NationalCode,
                Address = _.Address,
                Study = _.Study,
                Diploma = _.Diploma,
                DateOfBirth = _.DateOfBirth,
                GroupOfCourse = _.GroupOfCourse,
                Mobile = new GetMobileDto(_.Mobile.CountryCallingCode, _.Mobile.MobileNumber)
            }).FirstOrDefault()!;
    }

    public void Delete(Teacher teacher)
    {
        _context.Remove(teacher);
    }

    public void Update(Teacher teacher)
    {
        _context.Update(teacher);
    }

    public Teacher? FindById(int id)
    {
        return _context.Teachers.FirstOrDefault(_ => _.Id == id);
    }

    public bool IsExistNationalCode(string nationalCode)
    {
        return _context.Teachers.Any(_ => _.NationalCode == nationalCode);
    }
}