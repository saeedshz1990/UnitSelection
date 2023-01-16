using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Services.ChooseUnitServices.Contracts;

public interface ChooseUnitRepository :Repository
{
   void Add(ChooseUnit chooseUnit);
   Student GetStudent(int studentId);
   Student? FindByStudentId(int studentId);
}