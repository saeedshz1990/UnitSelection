using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.StudentServices.Contracts.Dto;

namespace UnitSelection.Services.StudentServices.Contracts;

public interface StudentService : Service
{
    Task Add(AddStudentDto dto);
    Task Update(UpdateStudentDto dto, int id);
    IList<GetStudentDto> GetAll();
    GetStudentByIdDto GetById(int id);
}