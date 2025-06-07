using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sneakahs.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPaymentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_Orders_OrderId",
                table: "PaymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetails_OrderId",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "StripeClientSecret",
                table: "PaymentDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentDetailsId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentDetailsId",
                table: "Orders",
                column: "PaymentDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentDetails_PaymentDetailsId",
                table: "Orders",
                column: "PaymentDetailsId",
                principalTable: "PaymentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentDetails_PaymentDetailsId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentDetailsId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentDetailsId",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "PaymentDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "PaymentDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripeClientSecret",
                table: "PaymentDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_OrderId",
                table: "PaymentDetails",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_Orders_OrderId",
                table: "PaymentDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
