using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class identityintegration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("" +
                "DELETE FROM \"Reservation\" " +
                "WhERE TRUE"
            );

            migrationBuilder.Sql("" +
              "DELETE FROM \"app_user\" " +
              "WhERE TRUE"
            );

            migrationBuilder.RenameColumn(
                name: "email",
                table: "app_user",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ParkingPlace",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "app_user",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "app_user",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "app_user",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "app_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "app_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "app_user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "app_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "app_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName1",
                table: "app_user",
                type: "text",
                nullable: true);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {



            migrationBuilder.DropColumn(
                name: "Name",
                table: "ParkingPlace");


            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "UserName1",
                table: "app_user");


            migrationBuilder.DropColumn(
                name: "Id",
                table: "app_user");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "app_user",
                newName: "email");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "app_user",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);


        }
    }
}
