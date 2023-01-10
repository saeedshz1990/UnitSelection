using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.Services.Terms.Contract;

public interface TermService : Service
{
    Task Add(AddTermDto dto);
    Task Update(int id, UpdateTermDto dto);
    IList<GetTermsDto> GetAll();
    Term GetById(int id);
    Task Delete(int id);
}