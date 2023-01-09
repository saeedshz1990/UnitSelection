using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
using UnitSelection.Services.Terms.Exceptions;

namespace UnitSelection.Services.Terms;

public class TermAppService : TermService
{
    private readonly TermRepository _repository;
    private readonly UnitOfWork _unitOfWork;

    public TermAppService(
        TermRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Add(AddTermDto dto)
    {
        var title = _repository.IsNameExist(dto.Name);

        StopIfNameIsExist(title);

        var term = new Term
        {
            Name = dto.Name
        };

        _repository.Add(term);
        await _unitOfWork.Complete();
    }

    public async Task Update(int id, UpdateTermDto dto)
    {
        var term = _repository.FindById(id);
        StopIfTermNotFound(term);

        var termName = _repository.IsNameExist(dto.Name);

        StopIfNameIsDuplicated(termName);

        term.Name = dto.Name;

        _repository.Update(term);
        await _unitOfWork.Complete();
    }

    public IList<GetTermsDto> GetAll()
    {
        return _repository.GetAll();
    }

    public Term GetById(int id)
    {
        return _repository.GetById(id);
    }

    private static void StopIfNameIsDuplicated(bool termName)
    {
        if (termName)
        {
            throw new TheNameTermsCanNotRepeatedException();
        }
    }

    private static void StopIfNameIsExist(bool title)
    {
        if (title)
        {
            throw new TheTermsNameIsExistException();
        }
    }

    private static void StopIfTermNotFound(Term term)
    {
        if (term == null)
        {
            throw new TermsNotFoundException();
        }
    }
}