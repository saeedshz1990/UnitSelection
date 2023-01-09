using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Services.Terms.Contract;

public interface TermRepository :Repository
{
    void Add(Term term);
    void Update(Term term);

    bool IsNameExist(string name);
    Term FindById(int id);
}