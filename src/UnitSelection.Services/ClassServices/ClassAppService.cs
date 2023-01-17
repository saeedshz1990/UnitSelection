﻿using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Terms;
using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.ClassServices.Contract;
using UnitSelection.Services.ClassServices.Contract.Dto;
using UnitSelection.Services.ClassServices.Exceptions;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Exceptions;

namespace UnitSelection.Services.ClassServices;

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
        var nameClass = _repository.IsNameExist(dto.Name, dto.TermId);

        StopIfClassNameIsExist(nameClass);

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
        StopIfClassNotFound(newClass);

        StopIfClassNameIsDuplicated(dto);

        var terms = _termRepository.FindById(dto.TermId);
        StopIfTermsNotFound(terms);

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

    public async Task Delete(int id)
    {
        var newClass = _repository.FindById(id);
        StopIfClassNotFound(newClass);

        var chooseUnit = _repository.IsExistInChooseUnit(id);

        StopIfClassSelectedByStudent(chooseUnit);
        
        _repository.Delete(newClass);
        await _unitOfWork.Complete();
    }

    private static void StopIfClassSelectedByStudent(bool chooseUnit)
    {
        if (chooseUnit)
        {
            throw new ClassSelectedByStudentException();
        }
    }

    private void StopIfClassNameIsDuplicated(UpdateClassDto dto)
    {
        if (_repository.IsNameExist(dto.Name, dto.TermId))
        {
            throw new ClassNameIsDuplicatedException();
        }
    }

    private static void StopIfClassNameIsExist(bool nameClass)
    {
        if (nameClass)
        {
            throw new ClassNameIsDuplicatedException();
        }
    }
    
    private static void StopIfTermsNotFound(Term terms)
    {
        if (terms == null)
        {
            throw new TermsNotFoundException();
        }
    }

    private static void StopIfClassNotFound(Class newClass)
    {
        if (newClass == null)
        {
            throw new ClassNotFoundException();
        }
    }
}