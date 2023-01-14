using UnitSelection.Services.Classes.Contract.Dto;

namespace UnitSelection.TestTools.ClassTestTools;

public class AddClassDtoBuilder
{
    private AddClassDto _dto;

    public AddClassDtoBuilder()
    {
        _dto = new AddClassDto
        {
            Name = "dummy",
            TermId = 1,
        };
    }
    
    public AddClassDtoBuilder WithName(string name)
    {
        _dto.Name = name;
        return this;
    }

    public AddClassDtoBuilder WithTermId(int termId)
    {
        _dto.TermId = termId;
        return this;
    }

    public AddClassDto Build()
    {
        return _dto;
    }
}