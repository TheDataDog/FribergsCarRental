using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergsCarRental.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Adress_AdressId",
                table: "Customers");

            migrationBuilder.AlterColumn<int>(
                name: "AdressId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Adress_AdressId",
                table: "Customers",
                column: "AdressId",
                principalTable: "Adress",
                principalColumn: "AdressId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Adress_AdressId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "AdressId",
                table: "Customers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Adress_AdressId",
                table: "Customers",
                column: "AdressId",
                principalTable: "Adress",
                principalColumn: "AdressId");
        }
    }
}
