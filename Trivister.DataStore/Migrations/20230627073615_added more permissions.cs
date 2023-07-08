using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Trivister.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class addedmorepermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0834d4fc-a976-4428-b6f8-d47b832fad1a"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "a326a8ba-08bc-458e-89b7-f8a6234878ed", new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(7450), new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(7450) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "79ac1549-2cc2-4c85-852d-14f9a7f09e44", new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(7440), new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(7440) });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bcf0f8de-c8c3-44ee-9c67-df972d604cf2"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified" },
                values: new object[] { "93a5b098-1310-46ed-b69c-a5e1dcb72860", new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(7460), new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(7460) });

            migrationBuilder.UpdateData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"), new Guid("363b37a0-c306-4472-a405-4b576334cca0") },
                column: "LastModified",
                value: new DateTime(2023, 6, 27, 7, 36, 15, 175, DateTimeKind.Utc).AddTicks(70));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("363b37a0-c306-4472-a405-4b576334cca0"),
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified", "MiddleName", "PasswordHash", "PhoneNumber" },
                values: new object[] { "6c232942-ed53-4280-a7ca-17fa864eda69", new DateTime(2023, 6, 27, 7, 36, 15, 169, DateTimeKind.Utc).AddTicks(3300), new DateTime(2023, 6, 27, 7, 36, 15, 169, DateTimeKind.Utc).AddTicks(3300), null, "AQAAAAEAACcQAAAAEFiyf2evei9JIUrUSbC6GyOfPZ4u7sZjg+KmNN9cXzN0idAxtdIiOEFezm4dXY+qcA==", null });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800) });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedOn", "LastModified" },
                values: new object[] { new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800) });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 4, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), null, "Can invite user", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), null, "CanInviteUser" },
                    { 5, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), null, "Can View user", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), null, "CanViewUsers" },
                    { 6, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), null, "Can View Loans", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), null, "CanViewLoans" },
                    { 7, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), null, "Can Approve Loans", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6800), null, "CanApproveLoans" },
                    { 8, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "Can Reject Loans", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "CanRejectLoans" },
                    { 9, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "Can WriteOff Loans", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "CanWriteOffLoans" },
                    { 10, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "Can View Reports", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "CanViewReports" },
                    { 11, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "Can Download Reports", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "CanDownloadReports" },
                    { 12, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "Can Create Role", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "CanCreateRole" },
                    { 13, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "Can Update Role", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "CanUpdateRole" },
                    { 14, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "Can Add permissions to Role", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "CanAddPermissionsToRole" },
                    { 15, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "Can View Roles", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "CanViewRoles" },
                    { 16, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "Can View Tickets", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6810), null, "CanViewTickets" },
                    { 17, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "Can View Tickets", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "CanViewTickets" },
                    { 18, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "Can Open/Close Tickets", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "CanOpenOrCloseTicket" },
                    { 19, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "Can Respond to Tickets", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "CanRespondToTicket" },
                    { 20, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "Can View Configurations", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "CanViewConfigurations" },
                    { 21, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "Can Update Maker Checker Configurations", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "CanUpdateMakerCheckerConfiguration" },
                    { 22, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "Can Create Loan Configuration", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "CanCreateLoanConfiguration" },
                    { 23, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "Can Update Loan Configuration", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "CanUpdateLoanConfiguration" },
                    { 24, null, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "Can Delete Loan Configuration", false, new DateTime(2023, 6, 27, 7, 36, 15, 174, DateTimeKind.Utc).AddTicks(6820), null, "CanDeleteLoanConfiguration" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 24);

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
                columns: new[] { "ConcurrencyStamp", "CreatedOn", "LastModified", "MiddleName", "PasswordHash", "PhoneNumber" },
                values: new object[] { "b1ec75fc-029a-4641-8acf-cf9e0ad26ea5", new DateTime(2023, 6, 9, 11, 40, 32, 560, DateTimeKind.Utc).AddTicks(5150), new DateTime(2023, 6, 9, 11, 40, 32, 560, DateTimeKind.Utc).AddTicks(5150), "Oluwaseyi", "AQAAAAEAACcQAAAAEP1ckUwspPDCnCqd/PYrmRe5dr/rgRqONqAffMuVWN3OVkPMHh5ZlAzyByO8eSqeeQ==", "08122310370" });

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
    }
}
