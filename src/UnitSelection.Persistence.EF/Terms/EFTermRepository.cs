using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Terms; 
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;

namespace UnitSelection.Persistence.EF.Terms;

public class EFTermRepository : TermRepository
{
    private readonly EFDataContext _context;

    public EFTermRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(Term term)
    {
        _context.Add(term);
    }

    public void Update(Term term)
    {
        _context.Update(term);
    }

    public Term GetById(int id)
    {
        return _context.Terms
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
        return _context.Terms.Select(_ => new GetTermsDto
        {
            Name = _.Name,
            StartDate = _.StartDate,
            EndDate = _.EndDate
        }).ToList();
    }

    public void Delete(Term term)
    {
        _context.Remove(term);
    }

    public bool IsNameExist(string name)
    {
        return _context.Terms.Any(_ => _.Name == name);
    }

    public Term FindById(int id)
    {
        return _context.Terms
            .FirstOrDefault(_ => _.Id == id)!;
    }

    public async Task<bool> CheckEndDate(DateTime startDate)
    {
        var result = await _context.Terms
            .AnyAsync(_ => _.EndDate <= startDate);
        return result;
    }
}