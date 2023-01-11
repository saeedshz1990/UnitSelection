using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Classes.Contract;
using UnitSelection.Services.Classes.Contract.Dto;
using UnitSelection.Services.Classes.Exceptions;

namespace UnitSelection.Services.Classes;

public class ClassAppService :ClassService
{
    private readonly ClassRepository _repository;
    private readonly UnitOfWork _unitOfWork;
    
    public ClassAppService(
        ClassRepository repository,
        UnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
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
}