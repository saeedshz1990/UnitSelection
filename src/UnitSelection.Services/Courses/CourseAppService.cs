using UnitSelection.Entities.Courses;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Courses.Contract;
using UnitSelection.Services.Courses.Contract.Dto;
using UnitSelection.Services.Courses.Exceptions;

namespace UnitSelection.Services.Courses;

public class CourseAppService : CourseService
{
    private readonly CourseRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public CourseAppService(
        CourseRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Add(AddCourseDto dto)
    {
        StopIfCourseNameIsDuplicated(dto.Name);

        StopIfCourseUnitCountEqualByZero(dto.UnitCount);

        var course = new Course
        {
            Name = dto.Name,
            DayOfWeek = dto.DayOfWeek,
            UnitCount = dto.UnitCount,
            StartHour = dto.StartHour,
            EndHour = dto.EndHour,
            ClassId = dto.ClassId,
            GroupOfCourse = dto.GroupOfCourse
        };

        _repository.Add(course);
        await _unitOfWork.Complete();
    }

    public async Task Update(UpdateCourseDto dto, int id)
    {
        var course = _repository.FindById(id);
        StopIfCourseNotFound(course);

        StopIfCourseNameIsDuplicated(dto.Name);

        StopIfCourseUnitCountEqualByZero(dto.UnitCount);

        course.Name = dto.Name;
        course.DayOfWeek = dto.DayOfWeek;
        course.UnitCount = dto.UnitCount;
        course.StartHour = dto.StartHour;
        course.EndHour = dto.EndHour;
        course.ClassId = dto.ClassId;
        course.GroupOfCourse = dto.GroupOfCourse;

        _repository.Update(course);
        await _unitOfWork.Complete();
    }

    public IList<GetCourseDto> GetAll()
    {
        return _repository.GetAll();
    }

    public GetCourseByIdDto GetById(int id)
    {
        return _repository.GetById(id);
    }

    public GetCourseByClassIdDto GetByClassId(int classId)
    {
        return _repository.GetByClassId(classId);
    }

    public async Task Delete(int id)
    {
        var course = _repository.FindById(id);
        if (course == null)
        {
            throw new CourseNotFoundException();
        }

        _repository.Delete(course);
        await _unitOfWork.Complete();
    }

    private static void StopIfCourseUnitCountEqualByZero(int unitCount)
    {
        if (unitCount <= 0)
        {
            throw new CourseUnitCountCanNotBeZeroException();
        }
    }

    private void StopIfCourseNameIsDuplicated(string name)
    {
        if (_repository.IsCourseNameExist(name))
        {
            throw new TheCourseNameWithSameTeacherException();
        }
    }

    private static void StopIfCourseNotFound(Course course)
    {
        if (course == null)
        {
            throw new CourseNotFoundException();
        }
    }
}