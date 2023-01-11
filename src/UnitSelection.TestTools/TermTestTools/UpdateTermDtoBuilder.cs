using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.TestTools.TermTestTools;

public class UpdateTermDtoBuilder
{
    private readonly UpdateTermDto _dto;

    public UpdateTermDtoBuilder()
    {
        _dto = new UpdateTermDto
        {
            Name = "dummy",
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
    }
    
    public UpdateTermDtoBuilder WithName(string name)
    {
        _dto.Name = name;
        return this;
    }

    public UpdateTermDtoBuilder WithStartDate(DateTime starDate)
    {
        _dto.StartDate = starDate;
        return this;
    }

    public UpdateTermDtoBuilder WithEndDate(DateTime endDate)
    {
        _dto.EndDate = endDate;
        return this;
    }

    public UpdateTermDto Build()
    {
        return _dto;
    }
}