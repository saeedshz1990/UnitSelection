using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.TestTools.TermTestTools;

public class AddTermDtoBuilder
{
    private readonly AddTermDto _dto;

    public AddTermDtoBuilder()
    {
        _dto = new AddTermDto
        {
            Name = "dummy",
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
    }
    
    public AddTermDtoBuilder WithName(string name)
    {
        _dto.Name = name;
        return this;
    }

    public AddTermDtoBuilder WithStartDate(DateTime starDate)
    {
        _dto.StartDate = starDate;
        return this;
    }

    public AddTermDtoBuilder WithEndDate(DateTime endDate)
    {
        _dto.EndDate = endDate;
        return this;
    }

    public AddTermDto Build()
    {
        return _dto;
    }
}