using Microsoft.AspNetCore.Mvc;
using UnitSelection.Services.Courses.Contract;
using UnitSelection.Services.Courses.Contract.Dto;

namespace UnitSelection.RestApi.Controllers.Courses;

[ApiController]
[Route("api/courses")]
public class CourseControllers : ControllerBase
{
    private readonly CourseService _service;

    public CourseControllers(CourseService service)
    {
        _service = service;
    }

    [HttpPost]
    public void Add(AddCourseDto dto)
    {
        _service.Add(dto);
    }

    [HttpPut("{id}")]
    public async Task Update(int id, UpdateCourseDto dto)
    {
        await _service.Update(dto, id);
    }

    [HttpGet]
    public IList<GetCourseDto> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public GetCourseByIdDto GetById(int id)
    {
        return _service.GetById(id);
    }

    [HttpGet("{classId}/get-course")]
    public GetCourseByClassIdDto GetByClassId(int classId)
    {
        return _service.GetByClassId(classId);
    }
    
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        _service.Delete(id);
    }
}