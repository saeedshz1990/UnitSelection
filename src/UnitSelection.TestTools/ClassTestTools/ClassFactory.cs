using UnitSelection.Entities.Classes;

namespace UnitSelection.TestTools.ClassTestTools;

public static class ClassFactory
{
    public static Class GenerateClass(string name = "dummy", int termId=1)
    {
        return new Class
        {
            Name = name,
            TermId = termId
        };
    }
}