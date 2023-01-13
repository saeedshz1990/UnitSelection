using FluentMigrator;

namespace UnitSelection.Migrations.Migrations;

[Migration(202301131347)]
public class _202301131347_AddedCoursesTable :Migration
{
    public override void Up()
    {
        Create.Table("Courses")
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
            .WithColumn("Name").AsString(30).NotNullable()
            .WithColumn("UnitCount").AsInt32().NotNullable()
            .WithColumn("StartHour").AsString(10).NotNullable()
            .WithColumn("EndHour").AsString(10).NotNullable()
            .WithColumn("DayOfWeek").AsString(15).NotNullable()
            .WithColumn("ClassId").AsInt32().NotNullable()
            .ForeignKey(
                "FK_Courses_Classes", 
                "Classes", 
                "Id");
    }

    public override void Down()
    {
        Delete.Table("Courses");
    }
}