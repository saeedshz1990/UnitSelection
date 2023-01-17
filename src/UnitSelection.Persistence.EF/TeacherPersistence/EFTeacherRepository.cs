using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Teachers;
using UnitSelection.Services.TeacherServices.Contract;
using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.Persistence.EF.TeacherPersistence;

public class EFTeacherRepository : TeacherRepository
{
    private readonly DbSet<Teacher> _teachers;
    private readonly DbSet<ChooseUnit> _chooseUnits;

    public EFTeacherRepository(EFDataContext context)
    {
        _teachers = context.Set<Teacher>();
        _chooseUnits = context.Set<ChooseUnit>();
    }

    public void Add(Teacher teacher)
    {
        _teachers.Add(teacher);
    }

    public IList<GetTeacherDto> GetAll()
    {
        return _teachers.Select(_ => new GetTeacherDto
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
        return _teachers
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
        return _teachers
            .Where(_ => _.CourseId == courseId)
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
        _teachers.Remove(teacher);
    }

    public void Update(Teacher teacher)
    {
        _teachers.Update(teacher);
    }

    public Teacher? FindById(int id)
    {
        return _teachers.FirstOrDefault(_ => _.Id == id);
    }

    public bool IsExistById(int id)
    {
        return _teachers.Any(_ => _.Id == id);
    }

    public bool IsExistChooseUnit(int teacherId)
    {
        return _chooseUnits.Any(_ => _.TeacherId == teacherId);
    }

    public bool IsExistByNationalCode(string nationalCode)
    {
        return _teachers.Any(_ => _.NationalCode == nationalCode);
    }
}