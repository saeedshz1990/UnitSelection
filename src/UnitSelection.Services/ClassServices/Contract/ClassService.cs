using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.ClassServices.Contract.Dto;

namespace UnitSelection.Services.ClassServices.Contract;

public interface ClassService : Service
{
    Task Add(AddClassDto dto);
    Task Update(UpdateClassDto dto, int id);
    IList<GetClassDto> GetAll();
    GetClassByIdDto GetById(int id);
    GetClassByTermIdDto? GetByTermId(int termId);
    Task Delete(int id);
}