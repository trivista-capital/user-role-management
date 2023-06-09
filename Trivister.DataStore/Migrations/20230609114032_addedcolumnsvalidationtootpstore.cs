using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trivister.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedcolumnsvalidationtootpstore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "OTPStore",
                type: "nvarchar(400)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OTP",
                table: "OTPStore",
                type: "nvarchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "OTPStore",
                type: "nvarchar(400)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0834d4fc-a976-4428-b6f8-d47b832fad1a"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "9288783c-2de2-42ec-ab4b-410bbfcd5e68", new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6760), new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6760) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "7118acb4-2cbc-499e-956b-5033cd77b58f", new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6750), new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6750) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bcf0f8de-c8c3-44ee-9c67-df972d604cf2"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "b9524bc4-7875-4970-9496-a3358082acbb", new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6770), new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6770) });

            migrationBuilder.UpdateData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"), new Guid("363b37a0-c306-4472-a405-4b576334cca0") },
                column: "LastModified",
                value: new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(8880));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("363b37a0-c306-4472-a405-4b576334cca0"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified", "PasswordHash" },
                values: new object[] { "b1ec75fc-029a-4641-8acf-cf9e0ad26ea5", new DateTime(2023, 6, 9, 11, 40, 32, 560, DateTimeKind.Utc).AddTicks(5150), new DateTime(2023, 6, 9, 11, 40, 32, 560, DateTimeKind.Utc).AddTicks(5150), "AQAAAAEAACcQAAAAEP1ckUwspPDCnCqd/PYrmRe5dr/rgRqONqAffMuVWN3OVkPMHh5ZlAzyByO8eSqeeQ==" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6240), new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6240) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6240), new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6240) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6240), new DateTime(2023, 6, 9, 11, 40, 32, 565, DateTimeKind.Utc).AddTicks(6240) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "OTPStore",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OTP",
                table: "OTPStore",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "OTPStore",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0834d4fc-a976-4428-b6f8-d47b832fad1a"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "545aca64-8db0-453f-9088-9de41afde6bb", new DateTime(2023, 6, 9, 11, 33, 0, 951, DateTimeKind.Utc), new DateTime(2023, 6, 9, 11, 33, 0, 951, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "70059592-e7ae-46e8-871a-a2d8bcfbe273", new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9990), new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9990) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bcf0f8de-c8c3-44ee-9c67-df972d604cf2"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "5bac2b31-ef66-48b8-95ff-398bce4e2576", new DateTime(2023, 6, 9, 11, 33, 0, 951, DateTimeKind.Utc).AddTicks(10), new DateTime(2023, 6, 9, 11, 33, 0, 951, DateTimeKind.Utc).AddTicks(10) });

            migrationBuilder.UpdateData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"), new Guid("363b37a0-c306-4472-a405-4b576334cca0") },
                column: "LastModified",
                value: new DateTime(2023, 6, 9, 11, 33, 0, 951, DateTimeKind.Utc).AddTicks(1710));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("363b37a0-c306-4472-a405-4b576334cca0"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified", "PasswordHash" },
                values: new object[] { "225c2799-3f4f-4c55-aa1c-53e4a1b8092f", new DateTime(2023, 6, 9, 11, 33, 0, 946, DateTimeKind.Utc).AddTicks(7020), new DateTime(2023, 6, 9, 11, 33, 0, 946, DateTimeKind.Utc).AddTicks(7020), "AQAAAAEAACcQAAAAEDDXL3RBHOu7htYEz0OmjXgBknwEPX8ktN2w/pvb16KW+XaqBRdogyYBG+AJbvAm7g==" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9530), new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9530) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9530), new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9530) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9530), new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9530) });
        }
    }
}
