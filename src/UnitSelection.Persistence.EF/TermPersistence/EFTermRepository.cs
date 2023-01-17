using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Terms;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.Persistence.EF.TermPersistence;

public class EFTermRepository : TermRepository
{
    private readonly DbSet<Term> _term;

    public EFTermRepository(EFDataContext context)
    {
        _term = context.Set<Term>();
    }

    public void Add(Term term)
    {
        _term.Add(term);
    }

    public void Update(Term term)
    {
        _term.Update(term);
    }

    public Term GetById(int id)
    {
        return _term
            .Select(_ => new Term
            {
                Id = _.Id,
                Name = _.Name,
                StartDate = _.StartDate,
                EndDate = _.EndDate
            }).FirstOrDefault(_ => _.Id == id)!;
    }

    public IList<GetTermsDto> GetAll()
    {
        return _term.Select(_ => new GetTermsDto
        {
            Id = _.Id,
            Name = _.Name,
            StartDate = _.StartDate,
            EndDate = _.EndDate
        }).ToList();
    }

    public void Delete(Term term)
    {
        _term.Remove(term);
    }

    public bool IsNameExist(string name)
    {
        return _term.Any(_ => _.Name == name);
    }

    public Term FindById(int id)
    {
        return _term
            .FirstOrDefault(_ => _.Id == id)!;
    }

    public async Task<bool> CheckEndDate(DateTime startDate)
    {
        var result = await _term
            .AnyAsync(_ => _.EndDate <= startDate);
        return result;
    }
}