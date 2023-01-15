using UnitSelection.Services.ClassServices.Contract.Dto;

namespace UnitSelection.TestTools.ClassTestTools;

public static class AddClassDtoFactory
{
    public static AddClassDto GenerateAddClassDto(string name = "dummy", int termId = 1)
    {
        return new AddClassDto
        {
            Name = name,
            TermId = termId
        };
    }
}