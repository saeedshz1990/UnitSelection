using Microsoft.AspNetCore.Mvc;
using UnitSelection.Entities.Classes;
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

    [HttpPut("{id}")]
    public async Task Update(int id, UpdateClassDto dto)
    {
        await _service.Update(dto, id);
    }

    [HttpGet]
    public IList<GetClassDto> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public GetClassByIdDto GetById(int id)
    {
        return _service.GetById(id);
    }

    [HttpGet("{termId}/get-by-term")]
    public GetClassByTermIdDto? GetByTermId(int termId)
    {
        return _service.GetByTermId(termId);
    }
}