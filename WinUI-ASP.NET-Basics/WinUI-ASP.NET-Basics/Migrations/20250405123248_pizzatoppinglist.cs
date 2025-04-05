using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinUI_ASP.NET_Basics.Migrations
{
    /// <inheritdoc />
    public partial class pizzatoppinglist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PizzaId1",
                table: "PizzaToppings",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderedAt",
                value: new DateTime(2025, 4, 5, 14, 32, 48, 216, DateTimeKind.Local).AddTicks(2581));

            migrationBuilder.UpdateData(
                table: "PizzaToppings",
                keyColumn: "Id",
                keyValue: 1,
                column: "PizzaId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "PizzaToppings",
                keyColumn: "Id",
                keyValue: 2,
                column: "PizzaId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "PizzaToppings",
                keyColumn: "Id",
                keyValue: 3,
                column: "PizzaId1",
                value: null);

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

            migrationBuilder.CreateIndex(
                name: "IX_PizzaToppings_PizzaId1",
                table: "PizzaToppings",
                column: "PizzaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PizzaToppings_Pizzas_PizzaId1",
                table: "PizzaToppings",
                column: "PizzaId1",
                principalTable: "Pizzas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PizzaToppings_Pizzas_PizzaId1",
                table: "PizzaToppings");

            migrationBuilder.DropIndex(
                name: "IX_PizzaToppings_PizzaId1",
                table: "PizzaToppings");

            migrationBuilder.DropColumn(
                name: "PizzaId1",
                table: "PizzaToppings");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderedAt",
                value: new DateTime(2025, 4, 4, 18, 34, 59, 416, DateTimeKind.Local).AddTicks(7468));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$0s/KO7Ana0PRpfd6Pq8PaOh3yH7Eqg7p45MclqcIgGC53whWE/ZCK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$ZPH2iZd/qelLLLETIo49j.QcshsXlYYu8uI9FSJR.PqL7TVNpPmOm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$Sgl8YhoPkcARcLSnMNPlC.u.xkbIi5VJw2VC6u/nzT/ODsVkhQutK");
        }
    }
}
