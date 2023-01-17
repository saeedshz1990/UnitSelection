using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;
using UnitSelection.Services.CourseServices.Contract;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts.Dto;

namespace UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers;

public class ChooseUnitHandlerAppService : ChooseUnitHandlerService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly ChooseUnitService _chooseUnitService;
    private readonly CourseService _courseService;
    public ChooseUnitHandlerAppService(
        UnitOfWork unitOfWork,
        ChooseUnitService service, 
        CourseService courseService)
    {
        _unitOfWork = unitOfWork;
        _chooseUnitService = service;
        _courseService = courseService;
    }

    public async Task Handle(AcceptChooseUnitDto dto)
    {
        var student = _chooseUnitService.GetStudent(dto.StudentId);

            var course = _courseService.GetCourseById(dto.CourseId);
            var teacher = _chooseUnitService.GetTeacher(dto.TeacherId);

            var chooseUnit = new AddChooseUnitDto()
            {
                StudentId = student.Id,
                CourseId = dto.CourseId,
                ClassId = course.ClassId,
                TermId = course.Class.TermId,
                TeacherId = teacher.Id
            };

            await _chooseUnitService.Add(chooseUnit);
            await _unitOfWork.Complete();
    }
}