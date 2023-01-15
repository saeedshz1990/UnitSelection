using Microsoft.AspNetCore.Mvc;
using UnitSelection.Services.StudentServices.Contracts;
using UnitSelection.Services.StudentServices.Contracts.Dto;

namespace UnitSelection.RestApi.Controllers.Students;

[ApiController]
[Route("api/students")]
public class StudentControllers : ControllerBase
{
    private readonly StudentService _service;

    public StudentControllers(StudentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task Add(AddStudentDto dto)
    {
        await _service.Add(dto);
    }

    [HttpPut("{id}")]
    public async Task Update(UpdateStudentDto dto, int id)
    {
        await _service.Update(dto, id);
    }
}