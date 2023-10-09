using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Migration_Add_Composite_Keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRental_Cars_CarId",
                table: "CarRental");

            migrationBuilder.DropForeignKey(
                name: "FK_CarRental_Customers_CustomerId",
                table: "CarRental");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarProducer_ProducerID",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Cars_CarId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Customers_CustomerId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_CarId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarRental",
                table: "CarRental");

            migrationBuilder.DropIndex(
                name: "IX_CarRental_CarId",
                table: "CarRental");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarProducer",
                table: "CarProducer");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "CarRentalId",
                table: "CarRental");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "CarRental",
                newName: "CarRentals");

            migrationBuilder.RenameTable(
                name: "CarProducer",
                newName: "Producers");

            migrationBuilder.RenameIndex(
                name: "IX_Review_CustomerId",
                table: "Reviews",
                newName: "IX_Reviews_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CarRental_CustomerId",
                table: "CarRentals",
                newName: "IX_CarRentals_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                columns: new[] { "CarId", "CustomerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarRentals",
                table: "CarRentals",
                columns: new[] { "CarId", "CustomerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Producers",
                table: "Producers",
                column: "ProducerID");

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentals_Cars_CarId",
                table: "CarRentals",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentals_Customers_CustomerId",
                table: "CarRentals",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Producers_ProducerID",
                table: "Cars",
                column: "ProducerID",
                principalTable: "Producers",
                principalColumn: "ProducerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Cars_CarId",
                table: "Reviews",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Customers_CustomerId",
                table: "Reviews",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRentals_Cars_CarId",
                table: "CarRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_CarRentals_Customers_CustomerId",
                table: "CarRentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Producers_ProducerID",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Cars_CarId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Customers_CustomerId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Producers",
                table: "Producers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarRentals",
                table: "CarRentals");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "Producers",
                newName: "CarProducer");

            migrationBuilder.RenameTable(
                name: "CarRentals",
                newName: "CarRental");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CustomerId",
                table: "Review",
                newName: "IX_Review_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CarRentals_CustomerId",
                table: "CarRental",
                newName: "IX_CarRental_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "ReviewId",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CarRentalId",
                table: "CarRental",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarProducer",
                table: "CarProducer",
                column: "ProducerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarRental",
                table: "CarRental",
                column: "CarRentalId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_CarId",
                table: "Review",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRental_CarId",
                table: "CarRental",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarRental_Cars_CarId",
                table: "CarRental",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarRental_Customers_CustomerId",
                table: "CarRental",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarProducer_ProducerID",
                table: "Cars",
                column: "ProducerID",
                principalTable: "CarProducer",
                principalColumn: "ProducerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Cars_CarId",
                table: "Review",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Customers_CustomerId",
                table: "Review",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
