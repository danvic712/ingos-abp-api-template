using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IngosAbpTemplate.EntityFrameworkCore.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ApplicationName = table.Column<string>(type: "varchar(96) CHARACTER SET utf8mb4", maxLength: 96, nullable: true),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    UserName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    TenantName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ImpersonatorUserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ImpersonatorTenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExecutionDuration = table.Column<int>(type: "int", nullable: false),
                    ClientIpAddress = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: true),
                    ClientId = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    CorrelationId = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    BrowserInfo = table.Column<string>(type: "varchar(512) CHARACTER SET utf8mb4", maxLength: 512, nullable: true),
                    HttpMethod = table.Column<string>(type: "varchar(16) CHARACTER SET utf8mb4", maxLength: 16, nullable: true),
                    Url = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    Exceptions = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", maxLength: 4000, nullable: true),
                    Comments = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    HttpStatusCode = table.Column<int>(type: "int", nullable: true),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpBackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    JobName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    JobArgs = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", maxLength: 1048576, nullable: false),
                    TryCount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NextTryTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastTryTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsAbandoned = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Priority = table.Column<byte>(type: "tinyint unsigned", nullable: false, defaultValue: (byte)15),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpBackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpAuditLogActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    AuditLogId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ServiceName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: true),
                    Parameters = table.Column<string>(type: "varchar(2000) CHARACTER SET utf8mb4", maxLength: 2000, nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExecutionDuration = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpAuditLogActions_AbpAuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "AbpAuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    AuditLogId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ChangeTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ChangeType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    EntityTenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    EntityId = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    EntityTypeFullName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityChanges_AbpAuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "AbpAuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityPropertyChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    EntityChangeId = table.Column<Guid>(type: "char(36)", nullable: false),
                    NewValue = table.Column<string>(type: "varchar(512) CHARACTER SET utf8mb4", maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(type: "varchar(512) CHARACTER SET utf8mb4", maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    PropertyTypeFullName = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                        column: x => x.EntityChangeId,
                        principalTable: "AbpEntityChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogActions_AuditLogId",
                table: "AbpAuditLogActions",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogActions_TenantId_ServiceName_MethodName_Execution~",
                table: "AbpAuditLogActions",
                columns: new[] { "TenantId", "ServiceName", "MethodName", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_UserId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "UserId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_AuditLogId",
                table: "AbpEntityChanges",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_TenantId_EntityTypeFullName_EntityId",
                table: "AbpEntityChanges",
                columns: new[] { "TenantId", "EntityTypeFullName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuditLogActions");

            migrationBuilder.DropTable(
                name: "AbpBackgroundJobs");

            migrationBuilder.DropTable(
                name: "AbpEntityPropertyChanges");

            migrationBuilder.DropTable(
                name: "AbpEntityChanges");

            migrationBuilder.DropTable(
                name: "AbpAuditLogs");
        }
    }
}
