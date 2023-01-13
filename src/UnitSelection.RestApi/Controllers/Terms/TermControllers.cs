using Microsoft.AspNetCore.Mvc;
using UnitSelection.Entities.Terms;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.RestApi.Controllers.Terms;

[ApiController]
[Route("api/terms")]
public class TermControllers : ControllerBase
{
    private readonly TermService _service;

    public TermControllers(TermService service)
    {
        _service = service;
    }

    [HttpPost]
    public void Add(AddTermDto dto)
    {
        _service.Add(dto);
    }

    [HttpPut("{id}")]
    public async Task Update(int id, UpdateTermDto dto)
    {
       await _service.Update(id, dto);
    }

    [HttpGet]
    public IList<GetTermsDto> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public Term GetById(int id)
    {
        return _service.GetById(id);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        _service.Delete(id);
    }
}