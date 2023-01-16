using UnitSelection.Persistence.EF;
using UnitSelection.Persistence.EF.ChooseUnitPersistence;
using UnitSelection.Persistence.EF.CoursesPersistence;
using UnitSelection.Persistence.EF.TeacherPersistence;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.CourseServices;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers;
using UnitSelection.Services.Handler.CommandHandlers.ChooseUnitHandlers.Contracts;
using UnitSelection.Services.TeacherServices;

namespace UnitSelection.TestTools.HandlerTestTools.AcceptChooseUnitHandler;

public static class HandlerServiceFactory
{
    public static ChooseUnitHandlerService GenerateHandlerService(EFDataContext context)
    {
        var unitOfWork = new EFUnitOfWork(context);
        var courseRepository = new EFCourseRepository(context);
        var courseService = new CourseAppService(courseRepository,unitOfWork);
        var chooseUnitRepository = new EFChooseUnitRepository(context);
        var teacherRepository = new EFTeacherRepository(context);
        var teacherService = new TeacherAppService(teacherRepository, unitOfWork);
        var chooseUnitService = new ChooseUnitAppService(unitOfWork, chooseUnitRepository,teacherRepository);
        return new ChooseUnitHandlerAppService(unitOfWork,chooseUnitService,courseService); 
    }
}