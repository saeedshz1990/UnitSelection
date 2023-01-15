using UnitSelection.Persistence.EF;
using UnitSelection.Persistence.EF.StudentPersistence;
using UnitSelection.Services.StudentServices;
using UnitSelection.Services.StudentServices.Contracts;

namespace UnitSelection.TestTools.StudentTestTools;

public static class StudentServiceFactory
{
    public static StudentService GenerateStudentService(EFDataContext context)
    {
        var unitOfWork = new EFUnitOfWork(context);
        var repository = new EFStudentRepository(context);
        return new StudentAppService(unitOfWork,repository);
    }
}