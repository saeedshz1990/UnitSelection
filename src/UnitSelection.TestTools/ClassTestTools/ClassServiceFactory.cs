using UnitSelection.Persistence.EF;
using UnitSelection.Persistence.EF.Classes;
using UnitSelection.Persistence.EF.Terms;
using UnitSelection.Services.Classes;
using UnitSelection.Services.Classes.Contract;

namespace UnitSelection.TestTools.ClassTestTools;

public static class ClassServiceFactory
{
    public static ClassService GenerateClassService(EFDataContext context)
    {
        var unitOfWork = new EFUnitOfWork(context);
        var repository = new EFClassRepository(context);
        var termRepository = new EFTermRepository(context);
        return new ClassAppService(repository, unitOfWork, termRepository);
    }
}