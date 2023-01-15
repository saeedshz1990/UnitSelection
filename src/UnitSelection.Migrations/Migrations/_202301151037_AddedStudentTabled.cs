using FluentMigrator;

namespace UnitSelection.Migrations.Migrations;

[Migration(202301151037)]
public class _202301151037_AddedStudentTabled:Migration
{
    public override void Up()
    {
        Create.Table("Students")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
            .WithColumn("FirstName").AsString(100).NotNullable()
            .WithColumn("LastName").AsString(200).NotNullable()
            .WithColumn("FatherName").AsString(50).NotNullable()
            .WithColumn("NationalCode").AsString(11).NotNullable()
            .WithColumn("DateOfBirth").AsString()
            .WithColumn("Address").AsString()
            .WithColumn("MobileNumber").AsString(10).NotNullable().Unique()
            .WithColumn("CountryCallingCode").AsString(3).NotNullable();
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}