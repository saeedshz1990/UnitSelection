using UnitSelection.Entities;
using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Contracts.Dto;
using UnitSelection.Services.StudentServices.Exceptions;

namespace UnitSelection.Services.StudentServices;

public class StudentAppService : StudentService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly StudentRepository _repository;

    public StudentAppService(
        UnitOfWork unitOfWork,
        StudentRepository repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task Add(AddStudentDto dto)
    {
        var nationalCode = _repository.IsExistNationalCode(dto.NationalCode);
        StopIfStudentIsExist(nationalCode);

        var student = new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            FatherName = dto.FatherName,
            NationalCode = dto.NationalCode,
            Address = dto.Address,
            DateOfBirth = dto.DateOfBirth,
            Mobile = new Mobile(
                dto.Mobile.CountryCallingCode,
                dto.Mobile.MobileNumber)
        };

        _repository.Add(student);
        await _unitOfWork.Complete();
    }
    
    public async Task Update(UpdateStudentDto dto, int id)
    {
        var student = _repository.FindById(id);
        StopIfStudentNotFound(student);


        student!.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.FatherName = dto.FatherName;
        student.Address = dto.Address;
        student.DateOfBirth = dto.DateOfBirth;

        _repository.Update(student);
        await _unitOfWork.Complete();
    }
    
    public IList<GetStudentDto> GetAll()
    {
        return _repository.GetAll();
    }

    public GetStudentByIdDto GetById(int id)
    {
        return _repository.GetById(id);
    }

    public async Task Delete(int id)
    {
        var student = _repository.FindById(id);
        StopIfStudentNotFound(student);

        var unit = _repository.IsExistChooseUnit(id);
        StopIfStudentHaveAnySelectedCourse(unit);

        _repository.Delete(student!);
        await _unitOfWork.Complete();
    }

    private static void StopIfStudentHaveAnySelectedCourse(bool unit)
    {
        if (unit)
        {
            throw new StudentHaveChooseUnitException();
        }
    }
    
    private static void StopIfStudentNotFound(Student? student)
    {
        if (student == null)
        {
            throw new StudentNotFoundException();
        }
    }
    
    private static void StopIfStudentIsExist(bool nationalCode)
    {
        if (nationalCode)
        {
            throw new StudentIsExistException();
        }
    }
}