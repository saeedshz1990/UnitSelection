using UnitSelection.Persistence.EF;
using UnitSelection.Persistence.EF.ClassPersistence;
using UnitSelection.Persistence.EF.TermPersistence;
using UnitSelection.Services.ClassServices;
using UnitSelection.Services.ClassServices.Contract;

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