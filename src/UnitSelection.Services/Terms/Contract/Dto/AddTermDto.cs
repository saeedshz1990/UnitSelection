namespace UnitSelection.Services.Terms.Contract.Dto;

public class AddTermDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}