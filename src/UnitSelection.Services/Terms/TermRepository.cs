using System.Collections.Concurrent;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Application;

namespace UnitSelection.Services.Terms;

public interface TermRepository :Repository
{
    void Add(Term term);

    bool IsNameExist(string name);
}