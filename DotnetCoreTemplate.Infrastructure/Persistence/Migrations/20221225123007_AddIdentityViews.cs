using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetCoreTemplate.Infrastructure.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class AddIdentityViews : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
                CREATE VIEW [Users]
                AS
                    SELECT [Id], [Email]
                    FROM [AspNetUsers]");

			migrationBuilder.Sql(@"
                CREATE VIEW [Roles]
                AS
                    SELECT [Id], [Name]
                    FROM [AspNetRoles]");

			migrationBuilder.Sql(@"
                CREATE VIEW [UserRoles]
                AS
                    SELECT [UserId], [RoleId]
                    FROM [AspNetUserRoles]");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DROP VIEW [Users]");

			migrationBuilder.Sql("DROP VIEW [Roles]");

			migrationBuilder.Sql("DROP VIEW [UserRoles]");
		}
	}
}