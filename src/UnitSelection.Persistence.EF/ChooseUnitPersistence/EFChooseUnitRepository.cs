using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;

namespace UnitSelection.Persistence.EF.ChooseUnitPersistence;

public class EFChooseUnitRepository : ChooseUnitRepository
{
    private readonly EFDataContext _context;

    public EFChooseUnitRepository(EFDataContext context)
    {
        _context = context;
    }

    public void Add(ChooseUnit chooseUnit)
    {
        _context.Add(chooseUnit);
    }

    public Student GetStudent(int studentId)
    {
        return _context.Students.FirstOrDefault(_ => _.Id == studentId)!;
    }

    public IList<GetChooseUnitDto> GetAll()
    {
        var result = (from chooseUnit in _context.ChooseUnits
            join teacher in _context.Teachers on chooseUnit.TeacherId equals teacher.Id into t
            from tcu in t.DefaultIfEmpty()
            join contextClass in _context.Classes on chooseUnit.ClassId equals contextClass.Id into c
            from ct in c.DefaultIfEmpty()
            join course in _context.Courses on chooseUnit.CourseId equals course.Id into co
            from ctco in co.DefaultIfEmpty()
            join student in _context.Students on chooseUnit.StudentId equals student.Id into s
            from sct in s.DefaultIfEmpty()
            select new GetChooseUnitDto
            {
                Id = chooseUnit.Id,
                TeacherFirstName = tcu.FirstName,
                TeacherLastName = tcu.LastName,
                CourseName = ctco.Name,
                CourseStartHour = ctco.StartHour,
                CourseEndHour = ctco.EndHour,
                CourseUnitCount = ctco.UnitCount,
                ClassName = ct.Name,
                TermName = ct.Term.Name,
                StudentFirstName = sct.FirstName,
                StudentLastName = sct.LastName,
                StudentNationalCode = sct.NationalCode
            }).ToList();

        return result;
    }

    public GetChooseUnitByIdDto GetById(int id)
    {
        var result = (from chooseUnit in _context.ChooseUnits
            where chooseUnit.Id == id
            join teacher in _context.Teachers on chooseUnit.TeacherId equals teacher.Id into t
            from tcu in t.DefaultIfEmpty()
            join contextClass in _context.Classes on chooseUnit.ClassId equals contextClass.Id into c
            from ct in c.DefaultIfEmpty()
            join course in _context.Courses on chooseUnit.CourseId equals course.Id into co
            from ctco in co.DefaultIfEmpty()
            join student in _context.Students on chooseUnit.StudentId equals student.Id into s
            from sct in s.DefaultIfEmpty()
            select new GetChooseUnitByIdDto
            {
                TeacherFirstName = tcu.FirstName,
                TeacherLastName = tcu.LastName,
                CourseName = ctco.Name,
                CourseStartHour = ctco.StartHour,
                CourseEndHour = ctco.EndHour,
                CourseUnitCount = ctco.UnitCount,
                ClassName = ct.Name,
                TermName = ct.Term.Name,
                StudentFirstName = sct.FirstName,
                StudentLastName = sct.LastName,
                StudentNationalCode = sct.NationalCode
            }).FirstOrDefault();

        return result;
    }

    public GetChooseUnitByTermId GetByTermId(int termId)
    {
        var result = (from chooseUnit in _context.ChooseUnits
            where chooseUnit.TermId == termId
            join teacher in _context.Teachers on chooseUnit.TeacherId equals teacher.Id into t
            from tcu in t.DefaultIfEmpty()
            join contextClass in _context.Classes on chooseUnit.ClassId equals contextClass.Id into c
            from ct in c.DefaultIfEmpty()
            join course in _context.Courses on chooseUnit.CourseId equals course.Id into co
            from ctco in co.DefaultIfEmpty()
            join student in _context.Students on chooseUnit.StudentId equals student.Id into s
            from sct in s.DefaultIfEmpty()
            select new GetChooseUnitByTermId
            {
                Id = chooseUnit.Id,
                TeacherFirstName = tcu.FirstName,
                TeacherLastName = tcu.LastName,
                CourseName = ctco.Name,
                CourseStartHour = ctco.StartHour,
                CourseEndHour = ctco.EndHour,
                CourseUnitCount = ctco.UnitCount,
                ClassName = ct.Name,
                TermName = ct.Term.Name,
                StudentFirstName = sct.FirstName,
                StudentLastName = sct.LastName,
                StudentNationalCode = sct.NationalCode
            }).FirstOrDefault();

        return result;
    }

    public void Delete(ChooseUnit unit)
    {
        _context.Remove(unit);
    }

    public ChooseUnit? FindById(int id)
    {
        return _context.ChooseUnits.FirstOrDefault(_ => _.Id == id);
    }

    public bool IsConflictCourse(int courseId, int classId)
    {
        return _context.Courses.Any(_ => _.Id == courseId
                                         && _.ClassId == classId);
    }

    public int GetCount(int studentId)
    {
        var count = (from chooseUnit in _context.ChooseUnits
            join student in _context.Students on studentId equals student.Id into scc
            from c in scc.DefaultIfEmpty()
            join course in _context.Courses on chooseUnit.CourseId equals course.Id into co
            select new Course
            {
                UnitCount = co.Sum(a => a.UnitCount)
            }).ToList().Count();

        return count;
    }
    public Student? FindByStudentId(int studentId)
    {
        return _context.Students.FirstOrDefault(_ => _.Id == studentId);
    }
}