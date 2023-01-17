using UnitSelection.Entities.Classes;

namespace UnitSelection.TestTools.ClassTestTools;

public class ClassBuilder
{
    private readonly Class _class;

    public ClassBuilder()
    {
        _class = new Class
        {
            Name = "dummy",
            TermId = 1,
        };
    }

    public ClassBuilder WithName(string name)
    {
        _class.Name = name;
        return this;
    }

    public ClassBuilder WithTermId(int termId)
    {
        _class.TermId = termId;
        return this;
    }

    public Class Build()
    {
        return _class;
    }
}