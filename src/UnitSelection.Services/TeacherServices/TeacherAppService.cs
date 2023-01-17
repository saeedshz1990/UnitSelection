using UnitSelection.Entities;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.TeacherServices.Contract;
using UnitSelection.Services.TeacherServices.Contract.Dto;
using UnitSelection.Services.TeacherServices.Exceptions;

namespace UnitSelection.Services.TeacherServices;

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
        var name = _repository.IsExistByNationalCode(dto.NationalCode);
        StopIfTeacherIsExist(name);

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
            Mobile = new Mobile(
                dto.Mobile.CountryCallingCode,
                dto.Mobile.MobileNumber)
        };

        _repository.Add(teacher);
        await _unitOfWork.Complete();
    }

    

    public IList<GetTeacherDto> GetAll()
    {
        return _repository.GetAll();
    }

    public GetTeacherByIdDto GetById(int id)
    {
        return _repository.GetById(id);
    }

    public GetTeacherByCourseIdDto GetByCourseId(int courseId)
    {
        return _repository.GetTeacherByCourseId(courseId);
    }

    public async Task Delete(int id)
    {
        var teacher = _repository.FindById(id);
        StopIfTeacherNotFound(teacher);

        var unit = _repository.IsExistChooseUnit(id);
        StopIfTeacherSelectedByStudent(unit);
        
        _repository.Delete(teacher!);
        await _unitOfWork.Complete();
    }

   

    public async Task Update(UpdateTeacherDto dto, int id)
    {
        var teacher = _repository.FindById(id);
        StopIfTeacherNotFound(teacher);

        teacher!.FirstName = dto.FirstName;
        teacher.LastName = dto.LastName;
        teacher.FatherName = dto.FatherName;
        teacher.Address = dto.Address;
        teacher.CourseId = dto.CourseId;
        teacher.Study = dto.Study;
        teacher.Diploma = dto.Diploma;
        teacher.DateOfBirth = dto.DateOfBirth;
        teacher.GroupOfCourse = dto.GroupOfCourse;

        _repository.Update(teacher);
        await _unitOfWork.Complete();
    }
    
    private static void StopIfTeacherIsExist(bool name)
    {
        if (name)
        {
            throw new TeacherIsExistException();
        }
    }
    
    private static void StopIfTeacherSelectedByStudent(bool unit)
    {
        if (unit)
        {
            throw new ThisTeacherSelectedByStudentException();
        }
    }

    private static void StopIfTeacherNotFound(Teacher? teacher)
    {
        if (teacher == null)
        {
            throw new TeacherNotFoundException();
        }
    }
}