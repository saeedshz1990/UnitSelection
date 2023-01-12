using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Classes.Contract;
using UnitSelection.Services.Classes.Contract.Dto;
using UnitSelection.Services.Classes.Exceptions;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Exceptions;

namespace UnitSelection.Services.Classes;

public class ClassAppService : ClassService
{
    private readonly ClassRepository _repository;
    private readonly TermRepository _termRepository;
    private readonly UnitOfWork _unitOfWork;

    public ClassAppService(
        ClassRepository repository,
        UnitOfWork unitOfWork,
        TermRepository termRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _termRepository = termRepository;
    }

    public async Task Add(AddClassDto dto)
    {
        var nameClass = _repository.IsNameExist(dto.Name);

        if (nameClass)
        {
            throw new ClassNameIsDuplicatedException();
        }

        var newClass = new Class
        {
            Name = dto.Name,
            TermId = dto.TermId
        };

        _repository.Add(newClass);
        await _unitOfWork.Complete();
    }

    public async Task Update(UpdateClassDto dto, int id)
    {
        var newClass = _repository.FindById(id);
        if (newClass == null)
        {
            throw new ClassNotFoundException();
        }

        if (_repository.IsNameExist(dto.Name))
        {
            throw new ClassNameIsDuplicatedException();
        }

        var terms = _termRepository.FindById(dto.TermId);
        if (terms == null)
        {
            throw new TermsNotFoundException();
        }

        newClass.Name = dto.Name;
        newClass.TermId = dto.TermId;

        _repository.Update(newClass);
        await _unitOfWork.Complete();
    }

    public IList<GetClassDto> GetAll()
    {
        return _repository.GetAll();
    }

    public GetClassByIdDto GetById(int id)
    {
        return _repository.GetById(id);
    }

    public GetClassByTermIdDto? GetByTermId(int termId)
    {
        return _repository.GetByTermId(termId);
    }
}