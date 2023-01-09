using UnitSelection.Entities.Terms;

namespace UnitSelection.TestTools.TermTestTools;

public static class TermServiceFactoryDto
{
    public static Term GenerateTerms(string name = "dummy")
    {
        return new Term
        {
            Name = name
        };
    }
}