using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;

namespace UnitSelection.Services.ChooseUnitServices.Contracts;

public interface ChooseUnitRepository : Repository
{
    void Add(ChooseUnit chooseUnit);
    Student GetStudent(int studentId);
    IList<GetChooseUnitDto> GetAll();
    GetChooseUnitByIdDto GetById(int id);
    GetChooseUnitByTermId GetByTermId(int termId);
    bool IsConflictCourse(int courseId, int classId);
    int GetCount(int studentId);
}