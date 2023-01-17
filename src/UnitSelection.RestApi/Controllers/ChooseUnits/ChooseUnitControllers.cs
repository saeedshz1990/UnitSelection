using Microsoft.AspNetCore.Mvc;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts.Dto;

namespace UnitSelection.RestApi.Controllers.ChooseUnits;

[ApiController]
[Route("api/chooseUnits")]
public class ChooseUnitControllers : ControllerBase
{
    private readonly ChooseUnitHandlerService _unitHandlerService;
    private readonly ChooseUnitService _chooseUnitService;

    public ChooseUnitControllers(
        ChooseUnitHandlerService unitHandlerService,
        ChooseUnitService chooseUnitService)
    {
        _unitHandlerService = unitHandlerService;
        _chooseUnitService = chooseUnitService;
    }

    [HttpPost]
    public async Task Add(AcceptChooseUnitDto dto)
    {
        await _unitHandlerService.Handle(dto);
    }

    [HttpGet]
    public IList<GetChooseUnitDto> GetAll()
    {
        return _chooseUnitService.GetAll();
    }

    [HttpGet("{id}")]
    public GetChooseUnitByIdDto GetById(int id)
    {
        return _chooseUnitService.GetById(id);
    }

    [HttpGet("{termId}/get-choose-unit")]
    public GetChooseUnitByTermId GetByTermId(int termId)
    {
        return _chooseUnitService.GetByTermId(termId);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        _chooseUnitService.Delete(id);
    }
}