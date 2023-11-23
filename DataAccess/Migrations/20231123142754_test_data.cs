using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class test_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "app_user",
                newName: "UserName");
            
            migrationBuilder.Sql("INSERT INTO public.app_user(\"UserName\") \n" +
                                "VALUES('TestUser')");

            migrationBuilder.Sql("INSERT INTO public.\"ParkingPlace\"(\"DisabledParking\") \n" +
                "Values(false)");


            migrationBuilder.Sql("INSERT INTO public.\"Reservation\"(\"UserId\", \"ParkingPlaceId\", \"Beginning\", \"Ending\") \n" +
                "Values(" +
                "(SELECT \"ID\" from app_user where \"UserName\"='TestUser' order by \"ID\" asc limit 1), " +
                "(SELECT \"ID\" from \"ParkingPlace\" order by \"ID\" asc limit 1), '" + DateTime.Now +"', '"+ DateTime.Now.AddHours(1) + "')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "app_user",
                newName: "Name");
        }
    }
}
