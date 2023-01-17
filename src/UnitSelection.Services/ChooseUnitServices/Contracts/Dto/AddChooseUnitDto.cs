namespace UnitSelection.Services.ChooseUnitServices.Contracts.Dto;

public class AddChooseUnitDto
{
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public int CourseId { get; set; }
    public int TermId { get; set; }
    public int TeacherId { get; set; }
}