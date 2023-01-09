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

        if (title)
        {
            throw new TheTermsNameIsExistException();
        }

        var term = new Term
        {
            Name = dto.Name
        };

        _repository.Add(term);
        await _unitOfWork.Complete();
    }
}