using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cortex.Module.Issues.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedWorkSpaceRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkSpaceRoles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Manages the workspace, projects, and members.", "TeamLead" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Builds and maintains server-side application logic.", "Backend Developer" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Builds and maintains user-facing interfaces.", "Frontend Developer" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Works on both server-side and client-side development.", "Full Stack Developer" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Builds and maintains iOS/Android mobile applications.", "Mobile Developer" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Handles testing and quality assurance processes.", "QA" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Manages infrastructure, deployment, and CI/CD pipelines.", "DevOps" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Handles business analysis and requirements management.", "BA" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Designs user experience and interface layouts.", "UI/UX Designer" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Produces visual designs and branding materials.", "Graphic Designer" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Designs website visual layout and styling.", "Web Designer" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Manages marketing strategy and campaigns.", "Marketing Specialist" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Owns product strategy and roadmap management.", "Product Manager" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Handles data analysis and reporting.", "Data Analyst" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Handles security audits and risk management.", "Security Engineer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "WorkSpaceRoles",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));
        }
    }
}
