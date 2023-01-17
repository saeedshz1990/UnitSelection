using UnitSelection.Services.ClassServices.Contract.Dto;

namespace UnitSelection.TestTools.ClassTestTools;

public class UpdateClassDtoBuilder
{
    private readonly UpdateClassDto _dto;

    public UpdateClassDtoBuilder()
    {
        _dto = new UpdateClassDto
        {
            Name = "dummy",
            TermId = 1,
        };
    }

    public UpdateClassDtoBuilder WithName(string name)
    {
        _dto.Name = name;
        return this;
    }

    public UpdateClassDtoBuilder WithTermId(int termId)
    {
        _dto.TermId = termId;
        return this;
    }

    public UpdateClassDto Build()
    {
        return _dto;
    }
}