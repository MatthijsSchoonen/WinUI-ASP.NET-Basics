using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinUI_ASP.NET_Basics.Migrations
{
    /// <inheritdoc />
    public partial class order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Pizzas",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderedAt",
                value: new DateTime(2025, 4, 5, 15, 59, 47, 85, DateTimeKind.Local).AddTicks(6113));

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 2,
                column: "OrderId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Pizzas",
                keyColumn: "Id",
                keyValue: 3,
                column: "OrderId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$nvBH8bcLyrv2frLXRllWk.y2CJRPsAj70ztBkLXogYc42KVd9BqP6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$d2OlzD37fdWX0HGmlxoXzuubp56IK4GgX17jEh6bU.zpEnY6h3TCS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$ZpTv3aE5ENf0nB13L1d7J.tAR7aGpNv/8TXBiEAWkQ8BxlngKucai");

            migrationBuilder.CreateIndex(
                name: "IX_Pizzas_OrderId",
                table: "Pizzas",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pizzas_Orders_OrderId",
                table: "Pizzas",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pizzas_Orders_OrderId",
                table: "Pizzas");

            migrationBuilder.DropIndex(
                name: "IX_Pizzas_OrderId",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Pizzas");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderedAt",
                value: new DateTime(2025, 4, 5, 14, 32, 48, 216, DateTimeKind.Local).AddTicks(2581));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$KQGk2SzU52t.eKnS1oOOs.e6w0esdF/HF0GXL.3rvg5l6mvxN5DoG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$vKi7pVt2CsE8PyR30xeLwOG6KkbVBrUJ2yvNrSAE8R/XSARVzRCk6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$B5iUAF5Mah3zyxqiwnHRi.YAD4eOD6PiNjqVYNkZtX6hsEwoNCNa2");
        }
    }
}
