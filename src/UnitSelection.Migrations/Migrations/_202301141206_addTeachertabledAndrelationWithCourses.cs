using FluentMigrator;

namespace UnitSelection.Migrations.Migrations;

[Migration(202301141206)]
public class _202301141206_addTeachertabledAndrelationWithCourses : Migration
{
    public override void Up()
    {
        Alter.Table("Courses")
            .AddColumn("GroupOfCourse").AsString();
        
        Create.Table("Teachers")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
            .WithColumn("FirstName").AsString(100).NotNullable()
            .WithColumn("LastName").AsString(200).NotNullable()
            .WithColumn("FatherName").AsString(50).NotNullable()
            .WithColumn("NationalCode").AsString(11).NotNullable()
            .WithColumn("DateOfBirth").AsString()
            .WithColumn("Address").AsString()
            .WithColumn("GroupOfCourse").AsString()
            .WithColumn("MobileNumber").AsString(10).NotNullable().Unique()
            .WithColumn("CountryCallingCode").AsString(3).NotNullable()
            .WithColumn("Diploma").AsString().NotNullable()
            .WithColumn("Study").AsString().NotNullable()
            .WithColumn("CourseId").AsInt32().NotNullable()
            .ForeignKey(
                "FK_Teachers_Courses",
                "Courses",
                "Id");
    }

    public override void Down()
    {
        Delete.Column("GroupOfCourse").FromTable("Courses");
        Delete.ForeignKey("FK_Teachers_Courses").OnTable("Teachers");
        Delete.Table("Teachers");
    }
}