using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts.Dto;

namespace UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts;

public interface ChooseUnitHandlerService : Service
{
    Task Handle(AcceptChooseUnitDto dto);
}