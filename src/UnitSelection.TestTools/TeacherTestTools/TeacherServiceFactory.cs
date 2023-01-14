using UnitSelection.Persistence.EF;
using UnitSelection.Persistence.EF.TeacherPersistence;
using UnitSelection.Services.Teachers;
using UnitSelection.Services.Teachers.Contract;

namespace UnitSelection.TestTools.TeacherTestTools;

public static class TeacherServiceFactory
{
    public static TeacherService GenerateTeacherService(EFDataContext context)
    {
        var unitOfWork = new EFUnitOfWork(context);
        var repository = new EFTeacherRepository(context);
        return new TeacherAppService(repository, unitOfWork);
    }
}