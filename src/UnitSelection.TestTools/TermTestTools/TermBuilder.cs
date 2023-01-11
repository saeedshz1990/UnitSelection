using UnitSelection.Entities.Terms;

namespace UnitSelection.TestTools.TermTestTools;

public class TermBuilder
{
    private readonly Term _term;

    public TermBuilder()
    {
        _term = new Term
        {
            Name = "dummy",
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
    }

    public TermBuilder WithName(string name)
    {
        _term.Name = name;
        return this;
    }

    public TermBuilder WithStartDate(DateTime starDate)
    {
        _term.StartDate = starDate;
        return this;
    }

    public TermBuilder WithEndDate(DateTime endDate)
    {
        _term.EndDate = endDate;
        return this;
    }

    public Term Build()
    {
        return _term;
    }
}