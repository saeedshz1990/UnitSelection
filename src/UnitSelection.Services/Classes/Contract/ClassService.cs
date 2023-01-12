﻿using UnitSelection.Infrastructure.Application;
using UnitSelection.Services.Classes.Contract.Dto;

namespace UnitSelection.Services.Classes.Contract;

public interface ClassService : Service
{
    Task Add(AddClassDto dto);
    Task Update(UpdateClassDto dto, int id);
    IList<GetClassDto> GetAll();
    GetClassByIdDto GetById(int id);
    GetClassByTermIdDto? GetByTermId(int termId);
}