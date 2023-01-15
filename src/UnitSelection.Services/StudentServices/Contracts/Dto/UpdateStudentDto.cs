using UnitSelection.Services.TeacherServices.Contract.Dto;

namespace UnitSelection.Services.StudentServices.Contracts.Dto;

public class UpdateStudentDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public GetMobileDto Mobile { get; set; }
}