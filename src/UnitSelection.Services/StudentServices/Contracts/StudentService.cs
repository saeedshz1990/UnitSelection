using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.StudentServices.Contracts.Dto;

namespace UnitSelection.Services.StudentServices.Contracts;

public interface StudentService :Service
{
    Task Add(AddStudentDto dto);
}