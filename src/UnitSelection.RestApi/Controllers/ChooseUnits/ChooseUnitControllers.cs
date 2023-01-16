using Microsoft.AspNetCore.Mvc;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts.Dto;

namespace UnitSelection.RestApi.Controllers.ChooseUnits;

[ApiController]
[Route("api/chooseUnits")]
public class ChooseUnitControllers : ControllerBase
{
    private readonly ChooseUnitHandlerService _unitHandlerService;

    public ChooseUnitControllers(ChooseUnitHandlerService unitHandlerService)
    {
        _unitHandlerService = unitHandlerService;
    }

    [HttpPost]
    public async Task Add(AcceptChooseUnitDto dto)
    {
        await _unitHandlerService.Handle(dto);
    }
}