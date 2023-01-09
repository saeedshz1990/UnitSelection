using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.TestTools.TermTestTools;

public static class AddTermServiceFactory
{
    public static AddTermDto GenerateAddTerm(string name = "dummy")
    {
        return new AddTermDto
        {
            Name = name
        };
    }
}