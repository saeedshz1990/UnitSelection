using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Services.ChooseUnitServices.Contracts;

public interface ChooseUnitService :Service
{
    Task Add(ChooseUnit chooseUnit);
    Teacher GetTeacher(int id);
    Student GetStudent(int studentId);
}