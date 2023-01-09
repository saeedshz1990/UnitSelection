using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitSelection.Infrastructure.Test;
using UnitSelection.Persistence.EF;
using UnitSelection.Services.Terms.Contract;
using UnitSelection.Services.Terms.Exceptions;
using UnitSelection.TestTools.TermTestTools;
using Xunit;

namespace UnitSelection.Services.Test.Unit.Terms;

public class TermServiceTests
{
    private readonly EFDataContext _context;
    private readonly TermService _sut;

    public TermServiceTests()
    {
        _context = new EFInMemoryDatabase().CreateDataContext<EFDataContext>();
        _sut = TermServiceFactory.GenerateTermService(_context);
    }

    [Fact]
    public async Task Add_add_term_properly()
    {
        var dto = AddTermServiceFactory.GenerateAddTerm();

        await _sut.Add(dto);

        var actualResult = await _context.Terms.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(dto.Name);
    }

    [Fact]
    public async Task Add_throw_exception_when_name_is_exist_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();

        _context.Manipulate(_ => _.Terms.Add(term));

        var dto = AddTermServiceFactory.GenerateAddTerm();

        var _actionResult = () => _sut.Add(dto);

        await _actionResult
            .Should()
            .ThrowExactlyAsync<TheTermsNameIsExistException>();
    }

    [Fact]
    public async Task Update_update_term_properly()
    {
        var term = TermServiceFactoryDto.GenerateTerms();
        _context.Manipulate(_ => _.Terms.Add(term));
        var dto = UpdateTermServiceFactoryDto.GenerateUpdateDto("updatedDummy");

        await _sut.Update(term.Id, dto);
        
        var actualResult = await _context.Terms.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(dto.Name);
    }

    [Theory]
    [InlineData(-1)]
    public async Task Update_throw_exception_when_Terms_not_found_properly(int invalidId)
    {
        var dto = UpdateTermServiceFactoryDto.GenerateUpdateDto("updatedDummy");

        var actualResult = async () => await _sut.Update(invalidId, dto);

        await actualResult.Should()
            .ThrowExactlyAsync<TermsNotFoundException>();
    }

    [Fact]
    public async Task Update_throw_exception_when_terms_name_is_duplicated_properly()
    {
        var firstTerm = TermServiceFactoryDto.GenerateTerms("firstDummy");
        _context.Manipulate(_ => _.Terms.Add(firstTerm));
        var secondTerm = TermServiceFactoryDto.GenerateTerms("secondDummy");
        _context.Manipulate(_ => _.Terms.Add(secondTerm));
        var dto = UpdateTermServiceFactoryDto.GenerateUpdateDto("secondDummy");

        var actualResult = async () => await _sut.Update(firstTerm.Id,dto);
        
        await actualResult.Should()
            .ThrowExactlyAsync<TheNameTermsCanNotRepeatedException>();
    }
}