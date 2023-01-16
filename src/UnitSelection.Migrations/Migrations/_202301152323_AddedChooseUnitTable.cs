using FluentMigrator;

namespace UnitSelection.Migrations.Migrations;

[Migration(202301152323)]
public class _202301152323_AddedChooseUnitTable: Migration
{
    public override void Up()
    {
        Create.Table("ChooseUnits")
            .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
            .WithColumn("StudentId").AsInt32().NotNullable()
            .WithColumn("ClassId").AsInt32().NotNullable()
            .WithColumn("CourseId").AsInt32().NotNullable()
            .WithColumn("TeacherId").AsInt32().NotNullable()
            .WithColumn("TermId").AsInt32().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("ChooseUnits");
    }
}