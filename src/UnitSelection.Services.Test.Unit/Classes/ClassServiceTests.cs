﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Entities.Classes;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Classes.Contract;
using UnitSelection.Services.Classes.Contract.Dto;
using UnitSelection.Services.Classes.Exceptions;
using UnitSelection.Services.Terms.Exceptions;
using UnitSelection.TestTools.ClassTestTools;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Services.Test.Unit.Classes;

public class ClassServiceTests
{
    private readonly EFDataContext _context;
    private readonly ClassService _sut;

    public ClassServiceTests()
    {
        _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
        _sut = ClassServiceFactory.GenerateClassService(_context);
    }

    [Fact]
    public async Task Add_add_Class_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var dto = new AddClassDto
        {
            Name = "101",
            TermId = term.Id
        };

        await _sut.Add(dto);

        var actualResult = await _context.Classes.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(dto.Name);
        actualResult.TermId.Should().Be(dto.TermId);
    }

    [Fact]
    public async Task Add_throw_exception_when_name_is_duplicated_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _context.Add(term));
        var newClass = new Class
        {
            Name = "101",
            TermId = term.Id
        };
        _context.Manipulate(_ => _.Add(newClass));
        var dto = new AddClassDto
        {
            Name = "101",
            TermId = term.Id
        };

        var actualResult = async () => await _sut.Add(dto);

        await actualResult.Should()
            .ThrowExactlyAsync<ClassNameIsDuplicatedException>();
    }

    [Fact]
    public async Task Update_update_class_properly()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _.Add(term));
        var firstClass = ClassFactory.GenerateClass("fisrtDummy", term.Id);
        _context.Manipulate(_ => _.Add(firstClass));
        var dto = UpdateClassDtoFactory
            .GenerateUpdateClassDto("secondDummy", term.Id);

        await _sut.Update(dto, firstClass.Id);

        var actualResult = await _context.Classes.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(dto.Name);
        actualResult.TermId.Should().Be(dto.TermId);
    }

    [Fact]
    public async Task Update_throw_exception_when_class_name_is_duplicated()
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _.Add(term));
        var firstClass = ClassFactory.GenerateClass("firstDummy", term.Id);
        _context.Manipulate(_ => _.Add(firstClass));
        var secondClass = ClassFactory.GenerateClass("secondDummy", term.Id);
        _context.Manipulate(_ => _.Add(secondClass));
        var dto = UpdateClassDtoFactory
            .GenerateUpdateClassDto("secondDummy", term.Id);

        var actualResult = async () => await _sut.Update(dto, firstClass.Id);

        await actualResult.Should()
            .ThrowExactlyAsync<ClassNameIsDuplicatedException>();
    }

    [Theory]
    [InlineData(-1)]
    public async Task Update_throw_exception_class_not_found_properly(int invalidId)
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _.Add(term));
        var dto = UpdateClassDtoFactory
            .GenerateUpdateClassDto("secondDummy", term.Id);

        var actualResult = async () => await _sut.Update(dto, invalidId);

        await actualResult.Should()
            .ThrowExactlyAsync<ClassNotFoundException>();
    }

    [Theory]
    [InlineData(-1)]
    public async Task Update_throw_exception_when_term_not_found_properly(int invalidTermId)
    {
        var term = new TermBuilder().Build();
        _context.Manipulate(_ => _.Add(term));
        var firstClass = ClassFactory.GenerateClass("firstDummy", term.Id);
        _context.Manipulate(_ => _.Add(firstClass));
        var dto = UpdateClassDtoFactory
            .GenerateUpdateClassDto("secondDummy", invalidTermId);

        var actualResult = async () => await _sut.Update(dto, firstClass.Id);

        await actualResult.Should()
            .ThrowExactlyAsync<TermsNotFoundException>();
    }
}