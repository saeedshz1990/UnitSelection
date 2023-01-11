using Microsoft.AspNetCore.Mvc;
using UnitSelection.Services.Classes.Contract;
using UnitSelection.Services.Classes.Contract.Dto;

namespace UnitSelection.RestApi.Controllers.Classes;

[ApiController]
[Route("api/classes")]
public class ClassControllers : ControllerBase
{
    private readonly ClassService _service;

    public ClassControllers(ClassService service)
    {
        _service = service;
    }

    [HttpPost]
    public async void Add(AddClassDto dto)
    {
        await _service.Add(dto);
    }
}