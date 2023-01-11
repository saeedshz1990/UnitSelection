using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.Services.Terms.Contract;

public interface TermRepository :Repository
{
    void Add(Term term);
    void Update(Term term);
    Term GetById(int id);
    IList<GetTermsDto> GetAll();
    void Delete(Term term);
    bool IsNameExist(string name);
    Term FindById(int id);
    Task<bool> CheckEndDate(DateTime endDate);
}