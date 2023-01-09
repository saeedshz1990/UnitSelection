using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitSelection.Entities.Terms;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.RestApi.Controllers.Terms;

[ApiController]
[Route("api/terms")]
// [Authorize]
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
}