using UnitSelection.Persistence.EF;
using UnitSelection.Persistence.EF.ChooseUnitPersistence;
using UnitSelection.Persistence.EF.TeacherPersistence;
using UnitSelection.Services.ChooseUnitServices;
using UnitSelection.Services.ChooseUnitServices.Contracts;

namespace UnitSelection.TestTools.ChooseUnitTestTools;

public static class ChooseUnitServiceFactory
{
    public static ChooseUnitService GenerateChooseUnitServiceFactory(EFDataContext context)
    {
        var unitOfWork = new EFUnitOfWork(context);
        var repository = new EFChooseUnitRepository(context);
        var teacherRepository = new EFTeacherRepository(context);
        return new ChooseUnitAppService(unitOfWork, repository, teacherRepository);
    }
}