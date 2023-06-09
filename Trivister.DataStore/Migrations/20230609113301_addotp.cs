using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Trivister.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addotp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OTPStore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPStore", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "70059592-e7ae-46e8-871a-a2d8bcfbe273", new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9990), new DateTime(2023, 6, 9, 11, 33, 0, 950, DateTimeKind.Utc).AddTicks(9990) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "LastModified", "LastModifiedBy", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0834d4fc-a976-4428-b6f8-d47b832fad1a"), "545aca64-8db0-453f-9088-9de41afde6bb", null, new DateTime(2023, 6, 9, 11, 33, 0, 951, DateTimeKind.Utc), null, "", false, new DateTime(2023, 6, 9, 11, 33, 0, 951, DateTimeKind.Utc), null, "Customer", "Customer" },
                    { new Guid("bcf0f8de-c8c3-44ee-9c67-df972d604cf2"), "5bac2b31-ef66-48b8-95ff-398bce4e2576", null, new DateTime(2023, 6, 9, 11, 33, 0, 951, DateTimeKind.Utc).AddTicks(10), null, "", false, new DateTime(2023, 6, 9, 11, 33, 0, 951, DateTimeKind.Utc).AddTicks(10), null, "Staff", "Staff" }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OTPStore");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0834d4fc-a976-4428-b6f8-d47b832fad1a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bcf0f8de-c8c3-44ee-9c67-df972d604cf2"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "e2c47a49-abfb-420d-a757-d6c8097be63c", new DateTime(2023, 6, 8, 15, 46, 48, 830, DateTimeKind.Utc).AddTicks(2730), new DateTime(2023, 6, 8, 15, 46, 48, 830, DateTimeKind.Utc).AddTicks(2730) });

            migrationBuilder.UpdateData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"), new Guid("363b37a0-c306-4472-a405-4b576334cca0") },
                column: "LastModified",
                value: new DateTime(2023, 6, 8, 15, 46, 48, 830, DateTimeKind.Utc).AddTicks(4860));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("363b37a0-c306-4472-a405-4b576334cca0"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified", "PasswordHash" },
                values: new object[] { "38b64755-cfc9-43e9-8453-f1177e3219aa", new DateTime(2023, 6, 8, 15, 46, 48, 825, DateTimeKind.Utc).AddTicks(1170), new DateTime(2023, 6, 8, 15, 46, 48, 825, DateTimeKind.Utc).AddTicks(1170), "AQAAAAEAACcQAAAAECecQo1e2BBjZpNVWqubfeBqjltmCdu0Gwi0HAv/ES1stwxj7RATKLShvKljq+jrSw==" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 8, 15, 46, 48, 830, DateTimeKind.Utc).AddTicks(2060), new DateTime(2023, 6, 8, 15, 46, 48, 830, DateTimeKind.Utc).AddTicks(2060) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 8, 15, 46, 48, 830, DateTimeKind.Utc).AddTicks(2060), new DateTime(2023, 6, 8, 15, 46, 48, 830, DateTimeKind.Utc).AddTicks(2060) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 8, 15, 46, 48, 830, DateTimeKind.Utc).AddTicks(2060), new DateTime(2023, 6, 8, 15, 46, 48, 830, DateTimeKind.Utc).AddTicks(2060) });
        }
    }
}
