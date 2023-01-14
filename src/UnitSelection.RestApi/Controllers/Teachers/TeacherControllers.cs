using Microsoft.AspNetCore.Mvc;
using UnitSelection.Services.Teachers.Contract;
using UnitSelection.Services.Teachers.Contract.Dto;

namespace UnitSelection.RestApi.Controllers.Teachers;

[ApiController]
[Route("api/teachers")]
public class TeacherControllers : ControllerBase
{
    private readonly TeacherService _service;

    public TeacherControllers(TeacherService service)
    {
        _service = service;
    }

    [HttpPost]
    public void Add(AddTeacherDto dto)
    {
        _service.Add(dto);
    }

    [HttpGet]
    public IList<GetTeacherDto> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public GetTeacherByIdDto GetById(int id)
    {
        return _service.GetById(id);
    }

    [HttpGet("{courseId}/get_teacher_by_course_id")]
    public GetTeacherByCourseIdDto GetByCourseId(int courseId)
    {
        return _service.GetByCourseId(courseId);
    }
}