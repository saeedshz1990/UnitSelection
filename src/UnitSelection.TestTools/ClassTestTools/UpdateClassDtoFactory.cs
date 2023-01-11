using UnitSelection.Services.Classes.Contract.Dto;

namespace UnitSelection.TestTools.ClassTestTools;

public static class UpdateClassDtoFactory
{
    public static UpdateClassDto GenerateUpdateClassDto(string name = "dummy", int id = 1)
    {
        return new UpdateClassDto
        {
            Name = name,
            TermId = id
        };
    }
}