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
        var name = _repository.IsCourseNameExist(dto.Name);
        if (name)
        {
            throw new TheCourseNameWithSameTeacherException();
        }

        var course = new Course
        {
            Name = dto.Name,
            DayOfWeek = dto.DayOfWeek,
            UnitCount = dto.UnitCount,
            StartHour = dto.StartHour,
            EndHour = dto.EndHour,
            ClassId = dto.ClassId
        };

        _repository.Add(course);
       await _unitOfWork.Complete();
    }
}