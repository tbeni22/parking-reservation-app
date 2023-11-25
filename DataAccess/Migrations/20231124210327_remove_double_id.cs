using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class remove_double_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FailureReport_app_user_UserId",
                table: "FailureReport");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_app_user_UserId",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_app_user",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "app_user");

            migrationBuilder.Sql("ALTER TABLE app_user ALTER COLUMN \"Id\" TYPE integer USING \"Id\"::integer ");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "app_user",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);



            migrationBuilder.AddPrimaryKey(
                name: "PK_app_user",
                table: "app_user",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FailureReport_app_user_UserId",
                table: "FailureReport",
                column: "UserId",
                principalTable: "app_user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_app_user_UserId",
                table: "Reservation",
                column: "UserId",
                principalTable: "app_user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FailureReport_app_user_UserId",
                table: "FailureReport");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_app_user_UserId",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_app_user",
                table: "app_user");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "app_user",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "app_user",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "valami",
                table: "app_user",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_app_user",
                table: "app_user",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FailureReport_app_user_UserId",
                table: "FailureReport",
                column: "UserId",
                principalTable: "app_user",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_app_user_UserId",
                table: "Reservation",
                column: "UserId",
                principalTable: "app_user",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
