namespace UnitSelection.Services.Classes.Contract.Dto;

public class GetClassDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TermId { get; set; }
}