using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.StudentServices.Exceptions;
using UnitSelection.Services.TeacherServices.Contract;

namespace UnitSelection.Services.ChooseUnitServices.Contracts;

public class ChooseUnitAppService :ChooseUnitService
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

    public async Task Add(ChooseUnit chooseUnit)
    {
        _repository.Add(chooseUnit);
        await _unitOfWork.Complete();
    }

    public Teacher GetTeacher(int id)
    {
        return _teacherRepository.FindById(id)!;
    }

    public Student GetStudent(int studentId)
    {
        return _repository.GetStudent(studentId);
    }
}