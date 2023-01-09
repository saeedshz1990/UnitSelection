using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.TestTools.TermTestTools;

public static class UpdateTermServiceFactoryDto
{
    public static UpdateTermDto GenerateUpdateDto(string name = "dummy")
    {
        return new UpdateTermDto
        {
            Name = name
        };
    }
}