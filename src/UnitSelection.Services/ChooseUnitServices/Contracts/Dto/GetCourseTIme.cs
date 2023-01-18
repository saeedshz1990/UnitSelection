using UnitSelection.Entities.Students;

namespace UnitSelection.Services.ChooseUnitServices.Contracts.Dto;

public class GetCourseTIme
{
    public string StartHour { get; set; }
    public string EndHour { get; set; }
}