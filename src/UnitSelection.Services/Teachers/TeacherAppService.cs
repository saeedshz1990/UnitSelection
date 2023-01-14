using UnitSelection.Entities;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Teachers.Contract;
using UnitSelection.Services.Teachers.Contract.Dto;
using UnitSelection.Services.Teachers.Exceptions;

namespace UnitSelection.Services.Teachers;

public class TeacherAppService : TeacherService
{
    private readonly TeacherRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public TeacherAppService(
        TeacherRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Add(AddTeacherDto dto)
    {
        var name = _repository.IsExistNationalCode(dto.NationalCode);
        if (name)
        {
            throw new TeacherIsExistException();
        }

        var teacher = new Teacher
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            FatherName = dto.FatherName,
            NationalCode = dto.NationalCode,
            DateOfBirth = dto.DateOfBirth,
            GroupOfCourse = dto.GroupOfCourse,
            Address = dto.Address,
            Diploma = dto.Diploma,
            Study = dto.Study,
            CourseId = dto.CourseId,
            Mobile = new Mobile(dto.Mobile.CountryCallingCode, dto.Mobile.MobileNumber)
        };

        _repository.Add(teacher);
        await _unitOfWork.Complete();
    }
}