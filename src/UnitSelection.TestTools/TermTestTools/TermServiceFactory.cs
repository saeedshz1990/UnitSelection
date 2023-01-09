using UnitSelection.Persistence.EF;
using UnitSelection.Persistence.EF.Terms;
using UnitSelection.Services.Terms;
using UnitSelection.Services.Terms.Contract;

namespace UnitSelection.TestTools.TermTestTools;

public static class TermServiceFactory
{
    public static TermService GenerateTermService(EFDataContext context)
    {
        var unitOfWork = new EFUnitOfWork(context);
        var repository = new EFTermRepository(context);
        return new TermAppService(repository, unitOfWork);
    }
}