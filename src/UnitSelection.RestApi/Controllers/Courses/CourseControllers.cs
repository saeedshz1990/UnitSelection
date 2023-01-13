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
}