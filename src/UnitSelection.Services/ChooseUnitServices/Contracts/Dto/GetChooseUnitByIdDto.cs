namespace UnitSelection.Services.ChooseUnitServices.Contracts.Dto;

public class GetChooseUnitByIdDto
{
    public string StudentFirstName { get; set; }= string.Empty;
    public string StudentLastName { get; set; }= string.Empty;
    public string StudentNationalCode { get; set; }= string.Empty;
    public string TermName { get; set; } = string.Empty;
    public string ClassName { get; set; }= string.Empty;
    public string TeacherFirstName { get; set; }= string.Empty;
    public string TeacherLastName { get; set; }= string.Empty;
    public string CourseName { get; set; }= string.Empty;
    public string CourseStartHour { get; set; }= string.Empty;
    public string CourseEndHour { get; set; }= string.Empty;
    public int CourseUnitCount { get; set; }
}