using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;
using UnitSelection.Services.ChooseUnitServices.Exceptions;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Exceptions;
using UnitSelection.Services.TeacherServices.Contract;

namespace UnitSelection.Services.ChooseUnitServices;

public class ChooseUnitAppService : ChooseUnitService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ChooseUnitRepository _repository;
    private readonly TeacherRepository _teacherRepository;

    public ChooseUnitAppService(
        UnitOfWork unitOfWork,
        ChooseUnitRepository repository,
        TeacherRepository teacherRepository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _teacherRepository = teacherRepository;
    }

    public async Task Add(AddChooseUnitDto dto)
    {
        var countOfCourse = _repository.GetCount(dto.StudentId);
        
        if (countOfCourse >20)
        {
            throw new CountOfCourseUnitMoreThanTwentyException();
        }

        var chaeckClass = _repository.IsConflictCourse(dto.CourseId,dto.ClassId);
        
        if (chaeckClass)
        {
            throw new ConflictingCourseHourException();
        }
        
        var chooseUnit = new ChooseUnit
        {
            ClassId = dto.ClassId,
            TeacherId = dto.TeacherId,
            CourseId = dto.CourseId,
            StudentId = dto.StudentId,
            TermId = dto.TermId
        };
        
        _repository.Add(chooseUnit);
        await _unitOfWork.Complete();
    }

    public Teacher GetTeacher(int id)
    {
        return _teacherRepository.FindById(id)!;
    }

    public IList<GetChooseUnitDto> GetAll()
    {
        return _repository.GetAll();
    }

    public GetChooseUnitByIdDto GetById(int id)
    {
        return _repository.GetById(id);
    }

    public GetChooseUnitByTermId GetByTermId(int termId)
    {
        return _repository.GetByTermId(termId);
    }

    public async Task Delete(int id)
    {
        var unit = _repository.FindById(id);
        if (unit ==null)
        {
            throw new ChooseUnitNotFoundException();
        }

        _repository.Delete(unit);
        await _unitOfWork.Complete();
    }

    public Student GetStudent(int studentId)
    {
        return _repository.GetStudent(studentId);
    }
}