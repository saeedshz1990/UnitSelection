﻿using UnitSelection.Persistence.EF;
using UnitSelection.Persistence.EF.CoursesPersistence;
using UnitSelection.Services.CourseServices;
using UnitSelection.Services.CourseServices.Contract;

namespace UnitSelection.TestTools.CourseTestTools;

public static class CourseServiceFactory
{
    public static CourseService GenerateCourseService(EFDataContext context)
    {
        var unitOfWork = new EFUnitOfWork(context);
        var repository = new EFCourseRepository(context);
        return new CourseAppService(repository, unitOfWork); 
    }
}