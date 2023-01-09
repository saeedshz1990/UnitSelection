using FluentMigrator;

namespace UnitSelection.Migrations.Migrations;

[Migration(202301090853)]
public class _202301090853_AddedTermsAndClassesTable:Migration
{
    public override void Up()
    {
        Create.Table("Terms")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(20);

        Create.Table("Classes")
            .WithColumn("Id").AsInt32().Identity().NotNullable().PrimaryKey()
            .WithColumn("Name").AsString(10)
            .WithColumn("TermId").AsInt32().NotNullable()
            .ForeignKey("FK_Classes_Terms",
                "Terms",
                "Id");
    }

    public override void Down()
    {
        Delete.Table("Terms");
        Delete.Table("Classes");
    }
}