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
        var dto = new AddTermDtoBuilder()
            .WithName("dummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();

        await _sut.Add(dto);

        var actualResult = await _context.Terms.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(dto.Name);
        actualResult!.StartDate.Should().Be(dto.StartDate);
        actualResult!.EndDate.Should().Be(dto.EndDate);
    }

    [Fact]
    public async Task Add_throw_exception_when_name_is_exist_properly()
    {
        var term = new TermBuilder()
            .WithName("dummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(term));
        var dto = new AddTermDtoBuilder()
            .WithName("dummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();

        var actionResult = () => _sut.Add(dto);

        await actionResult
            .Should()
            .ThrowExactlyAsync<TheTermsNameIsExistException>();
    }

    [Fact]
    public async Task
        Add_throw_exception_when_endDate_Equal_or_lowerthan_startdate_properly()
    {
        var dto = new AddTermDtoBuilder()
            .WithName("dummy")
            .WithStartDate(DateTime.UtcNow.AddDays(3))
            .WithEndDate(DateTime.UtcNow.AddDays(1))
            .Build();

        var actualResult = async () => await _sut.Add(dto);

        await actualResult
            .Should()
            .ThrowExactlyAsync<TheEndDateTermsCanNotLowerThanOrEqualStartDateException>();
    }

    [Fact]
    public async Task Update_update_term_properly()
    {
        var term = new TermBuilder()
            .WithName("dummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(term));
        var dto = new UpdateTermDtoBuilder()
            .WithName("updatedDummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();

        await _sut.Update(term.Id, dto);

        var actualResult = await _context.Terms.FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(dto.Name);
        actualResult!.StartDate.Should().Be(dto.StartDate);
        actualResult!.EndDate.Should().Be(dto.EndDate);
    }

    [Theory]
    [InlineData(-1)]
    public async Task Update_throw_exception_when_Terms_not_found_properly(int invalidId)
    {
        var dto = new UpdateTermDtoBuilder()
            .WithName("updatedDummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();

        var actualResult = async () => await _sut.Update(invalidId, dto);

        await actualResult.Should()
            .ThrowExactlyAsync<TermsNotFoundException>();
    }

    [Fact]
    public async Task Update_throw_exception_when_terms_name_is_duplicated_properly()
    {
        var firstTerm = new TermBuilder()
            .WithName("firstDummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(firstTerm));
        var secondTerm = new TermBuilder()
            .WithName("secondDummy")
            .WithStartDate(DateTime.UtcNow.AddMonths(3))
            .WithEndDate(DateTime.UtcNow.AddMonths(6))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(secondTerm));
        var dto = new UpdateTermDtoBuilder()
            .WithName("secondDummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();

        var actualResult = async () => await _sut.Update(firstTerm.Id, dto);

        await actualResult.Should()
            .ThrowExactlyAsync<TheNameTermsCanNotRepeatedException>();
    }

    [Fact]
    public async Task
        Update_throw_exception_when_endDate_Equal_or_lowerthan_startdate_properly()
    {
        var term = new TermBuilder()
            .WithName("dummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(term));
        var dto = new UpdateTermDtoBuilder()
            .WithName("updatedDummy")
            .WithStartDate(DateTime.UtcNow.AddDays(1))
            .WithEndDate(DateTime.UtcNow.Date)
            .Build();

        var actualResult = async () => await _sut.Update(term.Id, dto);

        await actualResult.Should()
            .ThrowExactlyAsync<TheEndDateTermsCanNotLowerThanOrEqualStartDateException>();
    }

    [Fact]
    public async Task Get_get_all_term_properly()
    {
        var firstTerm = new TermBuilder()
            .WithName("firstDummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(firstTerm));
        var secondTerm = new TermBuilder()
            .WithName("secondDummy")
            .WithStartDate(DateTime.UtcNow.AddMonths(3))
            .WithEndDate(DateTime.UtcNow.AddMonths(6))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(secondTerm));

        _sut.GetAll();

        var actualResult = await _context.Terms.ToListAsync();
        actualResult.Should().HaveCount(2);
    }

    [Fact]
    public async Task Get_get_all_when_Not_any_added_properly()
    {
        _sut.GetAll();

        var actualResult = await _context.Terms.ToListAsync();
        actualResult.Should().HaveCount(0);
    }

    [Fact]
    public async Task Get_get_by_id_properly()
    {
        var firstTerm = new TermBuilder()
            .WithName("firstDummy")
            .WithStartDate(DateTime.UtcNow.Date)
            .WithEndDate(DateTime.UtcNow.AddMonths(3))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(firstTerm));
        var secondTerm = new TermBuilder()
            .WithName("secondDummy")
            .WithStartDate(DateTime.UtcNow.AddMonths(3))
            .WithEndDate(DateTime.UtcNow.AddMonths(6))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(secondTerm));

        _sut.GetById(firstTerm.Id);

        var actualResult = await _context.Terms
            .FirstOrDefaultAsync();
        actualResult!.Name.Should().Be(firstTerm.Name);
        actualResult!.StartDate.Should().Be(firstTerm.StartDate);
        actualResult!.EndDate.Should().Be(firstTerm.EndDate);
    }

    [Fact]
    public async Task Delete_delete_term_properly()
    {
        var firstTerm = new TermBuilder()
            .WithName("firstDummy")
            .WithStartDate(DateTime.UtcNow.AddMonths(3))
            .WithEndDate(DateTime.UtcNow.AddMonths(6))
            .Build();
        _context.Manipulate(_ => _.Terms.Add(firstTerm));

        await _sut.Delete(firstTerm.Id);

        var actualResult = await _context.Terms.ToListAsync();
        actualResult.Should().HaveCount(0);
    }

    [Theory]
    [InlineData(-1)]
    public async Task Delete_throw_exception_when_term_not_found_properly(int invalidId)
    {
        var actualResult = () => _sut.Delete(invalidId);

        await actualResult.Should().ThrowExactlyAsync<TermsNotFoundException>();
    }
}