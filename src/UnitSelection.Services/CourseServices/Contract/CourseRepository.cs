using UnitSelection.Entities.Courses;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.CourseServices.Contract.Dto;

namespace UnitSelection.Services.CourseServices.Contract;

public interface CourseRepository : Repository
{
    void Add(Course course);
    void Update(Course dto);
    IList<GetCourseDto> GetAll();
    GetCourseByIdDto GetById(int id);
    GetCourseByClassIdDto GetByClassId(int classId);
    bool IsCourseNameExist(string name);
    Course FindById(int id);
    void Delete(Course course);
    Course GetCourseById(int id);

    bool IsExistInChooseUnit(int courseId);
}