using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialLowCortisolOperationalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    description = table.Column<string>(type: "character varying(520)", maxLength: 520, nullable: false),
                    severity = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    source_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    anomaly_id = table.Column<Guid>(type: "uuid", nullable: true),
                    reading_id = table.Column<Guid>(type: "uuid", nullable: true),
                    site_id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    resource_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    sla_minutes_to_acknowledge = table.Column<int>(type: "integer", nullable: false),
                    sla_minutes_to_resolve = table.Column<int>(type: "integer", nullable: false),
                    acknowledged_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    resolved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_alerts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "anomalies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reading_id = table.Column<Guid>(type: "uuid", nullable: false),
                    threshold_id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    resource_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    value = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    limit_value = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    severity = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    description = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    detected_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_anomalies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "consumption_readings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    resource_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    value = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    captured_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_consumption_readings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    serial_number = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    device_group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_devices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "incidents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    alert_id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    priority = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    title = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    description = table.Column<string>(type: "character varying(640)", maxLength: 640, nullable: false),
                    resolved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_incidents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_channels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(140)", maxLength: 140, nullable: false),
                    type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_notification_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    resource_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    last_reading_value = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sites",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    address = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sites", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "thresholds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_id = table.Column<Guid>(type: "uuid", nullable: true),
                    room_id = table.Column<Guid>(type: "uuid", nullable: true),
                    device_group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    sensor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    resource_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    @operator = table.Column<string>(name: "operator", type: "character varying(40)", maxLength: 40, nullable: false),
                    limit_value = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    severity = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_thresholds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "valves",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    resource_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_valves", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "alert_deliveries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    alert_id = table.Column<Guid>(type: "uuid", nullable: false),
                    channel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    channel_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    recipient_user_id = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    recipient_email = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    recipient_display_name = table.Column<string>(type: "character varying(140)", maxLength: 140, nullable: false),
                    message_title = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    message_description = table.Column<string>(type: "character varying(520)", maxLength: 520, nullable: false),
                    attempted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failure_reason = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_alert_deliveries", x => x.id);
                    table.ForeignKey(
                        name: "f_k_alert_deliveries_alerts_alert_id",
                        column: x => x.alert_id,
                        principalTable: "alerts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incident_actions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    incident_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action_type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    description = table.Column<string>(type: "character varying(520)", maxLength: 520, nullable: false),
                    performed_by = table.Column<string>(type: "character varying(140)", maxLength: 140, nullable: false),
                    performed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_incident_actions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_incident_actions_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incident_assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    incident_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assignee_id = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    assignee_name = table.Column<string>(type: "character varying(140)", maxLength: 140, nullable: false),
                    assigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_incident_assignments", x => x.id);
                    table.ForeignKey(
                        name: "f_k_incident_assignments_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_rooms", x => x.id);
                    table.ForeignKey(
                        name: "f_k_rooms_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "device_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    resource_type = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_device_groups", x => x.id);
                    table.ForeignKey(
                        name: "f_k_device_groups_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_alert_deliveries_alert_id",
                table: "alert_deliveries",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "i_x_alert_deliveries_attempted_at",
                table: "alert_deliveries",
                column: "attempted_at");

            migrationBuilder.CreateIndex(
                name: "i_x_alert_deliveries_channel_id",
                table: "alert_deliveries",
                column: "channel_id");

            migrationBuilder.CreateIndex(
                name: "i_x_alert_deliveries_channel_type",
                table: "alert_deliveries",
                column: "channel_type");

            migrationBuilder.CreateIndex(
                name: "i_x_alert_deliveries_status",
                table: "alert_deliveries",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_anomaly_id",
                table: "alerts",
                column: "anomaly_id");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_created_at",
                table: "alerts",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_device_group_id",
                table: "alerts",
                column: "device_group_id");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_device_id",
                table: "alerts",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_room_id",
                table: "alerts",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_sensor_id",
                table: "alerts",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_severity",
                table: "alerts",
                column: "severity");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_site_id",
                table: "alerts",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_status",
                table: "alerts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_detected_at",
                table: "anomalies",
                column: "detected_at");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_device_group_id",
                table: "anomalies",
                column: "device_group_id");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_device_id",
                table: "anomalies",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_reading_id",
                table: "anomalies",
                column: "reading_id");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_resource_type",
                table: "anomalies",
                column: "resource_type");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_room_id",
                table: "anomalies",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_sensor_id",
                table: "anomalies",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_severity",
                table: "anomalies",
                column: "severity");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_site_id",
                table: "anomalies",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "i_x_anomalies_status",
                table: "anomalies",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_consumption_readings_captured_at",
                table: "consumption_readings",
                column: "captured_at");

            migrationBuilder.CreateIndex(
                name: "i_x_consumption_readings_device_group_id",
                table: "consumption_readings",
                column: "device_group_id");

            migrationBuilder.CreateIndex(
                name: "i_x_consumption_readings_device_id",
                table: "consumption_readings",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_consumption_readings_resource_type",
                table: "consumption_readings",
                column: "resource_type");

            migrationBuilder.CreateIndex(
                name: "i_x_consumption_readings_room_id",
                table: "consumption_readings",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_consumption_readings_sensor_id",
                table: "consumption_readings",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "i_x_consumption_readings_site_id",
                table: "consumption_readings",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "i_x_consumption_readings_status",
                table: "consumption_readings",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_device_groups_created_at",
                table: "device_groups",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_device_groups_resource_type",
                table: "device_groups",
                column: "resource_type");

            migrationBuilder.CreateIndex(
                name: "i_x_device_groups_room_id",
                table: "device_groups",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_device_groups_room_id_name",
                table: "device_groups",
                columns: new[] { "room_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_device_groups_status",
                table: "device_groups",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_devices_created_at",
                table: "devices",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_devices_device_group_id",
                table: "devices",
                column: "device_group_id");

            migrationBuilder.CreateIndex(
                name: "i_x_devices_serial_number",
                table: "devices",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_devices_status",
                table: "devices",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_devices_type",
                table: "devices",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "i_x_incident_actions_action_type",
                table: "incident_actions",
                column: "action_type");

            migrationBuilder.CreateIndex(
                name: "i_x_incident_actions_incident_id",
                table: "incident_actions",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incident_actions_performed_at",
                table: "incident_actions",
                column: "performed_at");

            migrationBuilder.CreateIndex(
                name: "i_x_incident_assignments_assigned_at",
                table: "incident_assignments",
                column: "assigned_at");

            migrationBuilder.CreateIndex(
                name: "i_x_incident_assignments_assignee_id",
                table: "incident_assignments",
                column: "assignee_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incident_assignments_incident_id",
                table: "incident_assignments",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incident_assignments_is_active",
                table: "incident_assignments",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_alert_id",
                table: "incidents",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_created_at",
                table: "incidents",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_device_group_id",
                table: "incidents",
                column: "device_group_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_device_id",
                table: "incidents",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_priority",
                table: "incidents",
                column: "priority");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_room_id",
                table: "incidents",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_sensor_id",
                table: "incidents",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_site_id",
                table: "incidents",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_status",
                table: "incidents",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_notification_channels_created_at",
                table: "notification_channels",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_notification_channels_is_active",
                table: "notification_channels",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "i_x_notification_channels_type",
                table: "notification_channels",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "i_x_rooms_created_at",
                table: "rooms",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_rooms_site_id",
                table: "rooms",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "i_x_rooms_site_id_name",
                table: "rooms",
                columns: new[] { "site_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_rooms_status",
                table: "rooms",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_sensors_created_at",
                table: "sensors",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_sensors_device_id",
                table: "sensors",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_sensors_resource_type",
                table: "sensors",
                column: "resource_type");

            migrationBuilder.CreateIndex(
                name: "i_x_sensors_status",
                table: "sensors",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_sites_created_at",
                table: "sites",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_sites_name",
                table: "sites",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_sites_status",
                table: "sites",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "i_x_thresholds_created_at",
                table: "thresholds",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_thresholds_device_group_id",
                table: "thresholds",
                column: "device_group_id");

            migrationBuilder.CreateIndex(
                name: "i_x_thresholds_is_active",
                table: "thresholds",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "i_x_thresholds_resource_type",
                table: "thresholds",
                column: "resource_type");

            migrationBuilder.CreateIndex(
                name: "i_x_thresholds_room_id",
                table: "thresholds",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "i_x_thresholds_sensor_id",
                table: "thresholds",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "i_x_thresholds_severity",
                table: "thresholds",
                column: "severity");

            migrationBuilder.CreateIndex(
                name: "i_x_thresholds_site_id",
                table: "thresholds",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "i_x_valves_created_at",
                table: "valves",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_valves_device_id",
                table: "valves",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "i_x_valves_resource_type",
                table: "valves",
                column: "resource_type");

            migrationBuilder.CreateIndex(
                name: "i_x_valves_status",
                table: "valves",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert_deliveries");

            migrationBuilder.DropTable(
                name: "anomalies");

            migrationBuilder.DropTable(
                name: "consumption_readings");

            migrationBuilder.DropTable(
                name: "device_groups");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "incident_actions");

            migrationBuilder.DropTable(
                name: "incident_assignments");

            migrationBuilder.DropTable(
                name: "notification_channels");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropTable(
                name: "thresholds");

            migrationBuilder.DropTable(
                name: "valves");

            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "incidents");

            migrationBuilder.DropTable(
                name: "sites");
        }
    }
}
