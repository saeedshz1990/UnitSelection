using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using UnitSelection.Entities.ChooseUnits;
using UnitSelection.Entities.Classes;
using UnitSelection.Entities.Courses;
using UnitSelection.Entities.Students;
using UnitSelection.Entities.Teachers;
using UnitSelection.Services.ChooseUnitServices.Contracts;
using UnitSelection.Services.ChooseUnitServices.Contracts.Dto;

namespace UnitSelection.Persistence.EF.ChooseUnitPersistence;

public class EFChooseUnitRepository : ChooseUnitRepository
{
    private readonly DbSet<ChooseUnit> _chooseUnits;
    private readonly DbSet<Student> _students;
    private readonly DbSet<Teacher> _teachers;
    private readonly DbSet<Class> _class;
    private readonly DbSet<Course> _courses;

    public EFChooseUnitRepository(EFDataContext context)
    {
        _chooseUnits = context.Set<ChooseUnit>();
        _students = context.Set<Student>();
        _teachers = context.Set<Teacher>();
        _class = context.Set<Class>();
        _courses = context.Set<Course>();
    }

    public void Add(ChooseUnit chooseUnit)
    {
        _chooseUnits.Add(chooseUnit);
    }

    public Student GetStudent(int studentId)
    {
        return _students.FirstOrDefault(_ => _.Id == studentId)!;
    }

    public IList<GetChooseUnitDto> GetAll()
    {
        var result = (from chooseUnit in _chooseUnits
            join teacher in _teachers on chooseUnit.TeacherId equals teacher.Id into t
            from tcu in t.DefaultIfEmpty()
            join contextClass in _class on chooseUnit.ClassId equals contextClass.Id into c
            from ct in c.DefaultIfEmpty()
            join course in _courses on chooseUnit.CourseId equals course.Id into co
            from ctco in co.DefaultIfEmpty()
            join student in _students on chooseUnit.StudentId equals student.Id into s
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
        var result = (from chooseUnit in _chooseUnits
            where chooseUnit.Id == id
            join teacher in _teachers on chooseUnit.TeacherId equals teacher.Id into t
            from tcu in t.DefaultIfEmpty()
            join contextClass in _class on chooseUnit.ClassId equals contextClass.Id into c
            from ct in c.DefaultIfEmpty()
            join course in _courses on chooseUnit.CourseId equals course.Id into co
            from ctco in co.DefaultIfEmpty()
            join student in _students on chooseUnit.StudentId equals student.Id into s
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
        var result = (from chooseUnit in _chooseUnits
            where chooseUnit.TermId == termId
            join teacher in _teachers on chooseUnit.TeacherId equals teacher.Id into t
            from tcu in t.DefaultIfEmpty()
            join contextClass in _class on chooseUnit.ClassId equals contextClass.Id into c
            from ct in c.DefaultIfEmpty()
            join course in _courses on chooseUnit.CourseId equals course.Id into cou
            from co in cou.DefaultIfEmpty()
            join student in _students on chooseUnit.StudentId equals student.Id into s
            from sct in s.DefaultIfEmpty()
            select new GetChooseUnitByTermId
            {
                Id = chooseUnit.Id,
                TeacherFirstName = tcu.FirstName,
                TeacherLastName = tcu.LastName,
                CourseName = co.Name,
                CourseStartHour = co.StartHour,
                CourseEndHour = co.EndHour,
                CourseUnitCount = co.UnitCount,
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
        _chooseUnits.Remove(unit);
    }

    public ChooseUnit? FindById(int id)
    {
        return _chooseUnits.FirstOrDefault(_ => _.Id == id);
    }

    public List<Course> IsConflictCourse(int courseId, int classId)
    {
        // var result2 = (from unit in _context.ChooseUnits
        //     join course in _context.Courses on unit.CourseId equals courseId into co
        //     where unit.ClassId == classId
        //     from ddd in co.DefaultIfEmpty()
        //     select new GetCourseTIme()
        //     {
        //         StartHour = ddd.StartHour,
        //         EndHour = ddd.EndHour
        //     }).ToList();

        // var result = (
        //     from chooseUnit in _context.ChooseUnits
        //     where chooseUnit.ClassId == classId
        //     join course in _context.Classes on
        //         chooseUnit.ClassId equals course.Id into cc
        //     from cl in cc.DefaultIfEmpty()
        //     select new GetCourseTIme
        //     {
        //     }).ToList();

        // var choos = _context.ChooseUnits.Where(_ => _.ClassId == classId).ToList();
        // var newclass = _context.Classes.Where(_ => _.Id == classId).ToList();
        var newcourse = _courses
            .Where(_ => _.ClassId == classId)
            .Select(_ => new Course()
            {
                StartHour = _.StartHour,
                EndHour = _.EndHour
            }).ToList();

        return newcourse;
    }

    public int GetCount(int studentId)
    {
        var r = _chooseUnits
            .Where(_ => _.CourseId == studentId)
            .ToList();
        List<int> sum = _courses.Select(_ => _.UnitCount).ToList();
       
        int sss = 0;
        foreach (var item in sum)
        {
            sss += item;
        }

        return sss;
    }

    public Student? FindByStudentId(int studentId)
    {
        return _students.FirstOrDefault(_ => _.Id == studentId);
    }
}