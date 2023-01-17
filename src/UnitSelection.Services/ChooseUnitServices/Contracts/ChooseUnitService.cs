using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;

namespace UnitSelection.Services.ChooseUnitServices.Contracts;

public interface ChooseUnitService :Service
{
    Task Add(AddChooseUnitDto dto);
    Teacher GetTeacher(int id);
    IList<GetChooseUnitDto> GetAll();
    GetChooseUnitByIdDto GetById(int id);
    GetChooseUnitByTermId GetByTermId(int termId);
    Student GetStudent(int studentId);
}