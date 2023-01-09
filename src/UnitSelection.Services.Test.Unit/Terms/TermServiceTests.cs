using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Contract.Dto;
using UnitSelection.Services.Terms.Exceptions;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Services.Test.Unit.Terms;

public class TermServiceTests
{
    private readonly EFDataContext _context;
    private readonly TermService _sut;
    private Func<Task> _actionResult;

    public TermServiceTests()
    {
        _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [Fact]
    public async Task Add_add_term_properly()
    {
        var dto = new AddTermDto
        {
            Name = "dummy"
        };

        _sut.Add(dto);

        var actualResult = await _context.Terms.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(dto.Name);
    }

    [Fact]
    public async Task Add_throw_exception_when_name_is_exist_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();

        _context.Manipulate(_ => _.Terms.Add(term));

        var dto = new AddTermDto
        {
            Name = "dummy"
        };

        _actionResult = () => _sut.Add(dto);

        await _actionResult
            .Should()
            .ThrowExactlyAsync<TheTermsNameIsExistException>();
    }
}