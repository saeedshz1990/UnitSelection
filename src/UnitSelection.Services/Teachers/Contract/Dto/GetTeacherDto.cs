namespace UnitSelection.Services.Teachers.Contract.Dto;

public class GetTeacherDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string GroupOfCourse { get; set; } = string.Empty;
    public string Diploma { get; set; } = string.Empty;
    public string Study { get; set; } = string.Empty;
    public GetMobileDto Mobile { get; set; }
    public int CourseId { get; set; }
}