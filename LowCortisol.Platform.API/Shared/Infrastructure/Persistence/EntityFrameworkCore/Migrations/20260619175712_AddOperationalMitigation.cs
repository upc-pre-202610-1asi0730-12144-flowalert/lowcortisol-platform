using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationalMitigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "device_commands",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    valve_id = table.Column<Guid>(type: "uuid", nullable: true),
                    site_id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    incident_id = table.Column<Guid>(type: "uuid", nullable: true),
                    command_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    source = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    reason = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    requested_by = table.Column<string>(type: "character varying(140)", maxLength: 140, nullable: false),
                    requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    executed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failure_reason = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_device_commands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "valve_operations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    valve_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    incident_id = table.Column<Guid>(type: "uuid", nullable: true),
                    resource_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    previous_status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    target_status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    reason = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    source = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failure_reason = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_valve_operations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "command_audit_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_command_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    description = table.Column<string>(type: "character varying(520)", maxLength: 520, nullable: false),
                    performed_by = table.Column<string>(type: "character varying(140)", maxLength: 140, nullable: false),
                    performed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_command_audit_entries", x => x.id);
                    table.ForeignKey(
                        name: "f_k_command_audit_entries_device_commands_device_command_id",
                        column: x => x.device_command_id,
                        principalTable: "device_commands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "command_executions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_command_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    finished_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    result_message = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    failure_reason = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_command_executions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_command_executions_device_commands_device_command_id",
                        column: x => x.device_command_id,
                        principalTable: "device_commands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_command_audit_entries_action",
                table: "command_audit_entries",
                column: "action");

            migrationBuilder.CreateIndex(
                name: "i_x_command_audit_entries_device_command_id",
                table: "command_audit_entries",
                column: "device_command_id");

            migrationBuilder.CreateIndex(
                name: "i_x_command_audit_entries_performed_at",
                table: "command_audit_entries",
                column: "performed_at");

            migrationBuilder.CreateIndex(
                name: "i_x_command_executions_device_command_id",
                table: "command_executions",
                column: "device_command_id");

            migrationBuilder.CreateIndex(
                name: "i_x_command_executions_started_at",
                table: "command_executions",
                column: "started_at");

            migrationBuilder.CreateIndex(
                name: "i_x_command_executions_status",
                table: "command_executions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_command_type",
                table: "device_commands",
                column: "command_type");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_created_at",
                table: "device_commands",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_device_group_id",
                table: "device_commands",
                column: "device_group_id");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_device_id",
                table: "device_commands",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_incident_id",
                table: "device_commands",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_requested_at",
                table: "device_commands",
                column: "requested_at");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_room_id",
                table: "device_commands",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_site_id",
                table: "device_commands",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_status",
                table: "device_commands",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_device_commands_valve_id",
                table: "device_commands",
                column: "valve_id");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_created_at",
                table: "valve_operations",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_device_group_id",
                table: "valve_operations",
                column: "device_group_id");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_device_id",
                table: "valve_operations",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_incident_id",
                table: "valve_operations",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_reason",
                table: "valve_operations",
                column: "reason");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_requested_at",
                table: "valve_operations",
                column: "requested_at");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_room_id",
                table: "valve_operations",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_site_id",
                table: "valve_operations",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_status",
                table: "valve_operations",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_valve_operations_valve_id",
                table: "valve_operations",
                column: "valve_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "command_audit_entries");

            migrationBuilder.DropTable(
                name: "command_executions");

            migrationBuilder.DropTable(
                name: "valve_operations");

            migrationBuilder.DropTable(
                name: "device_commands");
        }
    }
}
