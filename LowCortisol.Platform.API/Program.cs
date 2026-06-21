using LowCortisol.Platform.API.DeviceControl.Application.Acl;
using LowCortisol.Platform.API.DeviceControl.Application.CommandServices;
using LowCortisol.Platform.API.DeviceControl.Application.Internal.Acl;
using LowCortisol.Platform.API.DeviceControl.Application.Internal.CommandServices;
using LowCortisol.Platform.API.DeviceControl.Application.Internal.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Application.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Monitoring.Application.CommandServices;
using LowCortisol.Platform.API.Monitoring.Application.Internal.CommandServices;
using LowCortisol.Platform.API.Monitoring.Application.Internal.QueryServices;
using LowCortisol.Platform.API.Monitoring.Application.QueryServices;
using LowCortisol.Platform.API.Monitoring.Domain.Repositories;
using LowCortisol.Platform.API.Monitoring.Domain.Services;
using LowCortisol.Platform.API.Notification.Application.Acl;
using LowCortisol.Platform.API.Notification.Application.CommandServices;
using LowCortisol.Platform.API.Notification.Application.Internal.Acl;
using LowCortisol.Platform.API.Notification.Application.Internal.CommandServices;
using LowCortisol.Platform.API.Notification.Application.Internal.QueryServices;
using LowCortisol.Platform.API.Notification.Application.QueryServices;
using LowCortisol.Platform.API.Notification.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Domain.Repositories;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Seed;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.OpenApi;
using LowCortisol.Platform.API.Workplace.Application.CommandServices;
using LowCortisol.Platform.API.Workplace.Application.Internal.CommandServices;
using LowCortisol.Platform.API.Workplace.Application.Internal.QueryServices;
using LowCortisol.Platform.API.Workplace.Application.QueryServices;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

using EfAppDbContext = LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.AppDbContext;
using EfAnomalyRepository = LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories.AnomalyRepository;
using EfAlertRepository = LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Repositories.AlertRepository;
using EfConsumptionReadingRepository = LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories.ConsumptionReadingRepository;
using EfDeviceGroupRepository = LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.EntityFrameworkCore.Repositories.DeviceGroupRepository;
using EfDeviceCommandRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories.DeviceCommandRepository;
using EfDeviceRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories.DeviceRepository;
using EfIncidentRepository = LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Repositories.IncidentRepository;
using EfNotificationChannelRepository = LowCortisol.Platform.API.Notification.Infrastructure.Persistence.EntityFrameworkCore.Repositories.NotificationChannelRepository;
using EfRoomRepository = LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.EntityFrameworkCore.Repositories.RoomRepository;
using EfSensorRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories.SensorRepository;
using EfSiteRepository = LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.EntityFrameworkCore.Repositories.SiteRepository;
using EfThresholdRepository = LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories.ThresholdRepository;
using EfUnitOfWork = LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories.UnitOfWork;
using EfValveOperationRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories.ValveOperationRepository;
using EfValveRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.EntityFrameworkCore.Repositories.ValveRepository;
using InMemoryAnomalyRepository = LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.InMemory.Repositories.AnomalyRepository;
using InMemoryAlertRepository = LowCortisol.Platform.API.Notification.Infrastructure.Persistence.InMemory.Repositories.AlertRepository;
using InMemoryAppDbContext = LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory.AppDbContext;
using InMemoryConsumptionReadingRepository = LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.InMemory.Repositories.ConsumptionReadingRepository;
using InMemoryDeviceGroupRepository = LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.InMemory.Repositories.DeviceGroupRepository;
using InMemoryDeviceCommandRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories.DeviceCommandRepository;
using InMemoryDeviceRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories.DeviceRepository;
using InMemoryIncidentRepository = LowCortisol.Platform.API.Notification.Infrastructure.Persistence.InMemory.Repositories.IncidentRepository;
using InMemoryNotificationChannelRepository = LowCortisol.Platform.API.Notification.Infrastructure.Persistence.InMemory.Repositories.NotificationChannelRepository;
using InMemoryRoomRepository = LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.InMemory.Repositories.RoomRepository;
using InMemorySensorRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories.SensorRepository;
using InMemorySiteRepository = LowCortisol.Platform.API.Workplace.Infrastructure.Persistence.InMemory.Repositories.SiteRepository;
using InMemoryThresholdRepository = LowCortisol.Platform.API.Monitoring.Infrastructure.Persistence.InMemory.Repositories.ThresholdRepository;
using InMemoryUnitOfWork = LowCortisol.Platform.API.Shared.Infrastructure.Persistence.InMemory.UnitOfWork;
using InMemoryValveOperationRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories.ValveOperationRepository;
using InMemoryValveRepository = LowCortisol.Platform.API.DeviceControl.Infrastructure.Persistence.InMemory.Repositories.ValveRepository;

const string frontendCorsPolicy = "FrontendCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddOpenApi();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? ["http://localhost:5173", "http://localhost:3000"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(frontendCorsPolicy, policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var persistenceProvider = builder.Configuration["Persistence:Provider"] ?? "PostgreSQL";
var useInMemory = persistenceProvider.Equals("InMemory", StringComparison.OrdinalIgnoreCase);

if (useInMemory)
{
    builder.Services.AddSingleton<InMemoryAppDbContext>();
    builder.Services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
    builder.Services.AddScoped<ISiteRepository, InMemorySiteRepository>();
    builder.Services.AddScoped<IRoomRepository, InMemoryRoomRepository>();
    builder.Services.AddScoped<IDeviceGroupRepository, InMemoryDeviceGroupRepository>();
    builder.Services.AddScoped<IDeviceRepository, InMemoryDeviceRepository>();
    builder.Services.AddScoped<ISensorRepository, InMemorySensorRepository>();
    builder.Services.AddScoped<IValveRepository, InMemoryValveRepository>();
    builder.Services.AddScoped<IDeviceCommandRepository, InMemoryDeviceCommandRepository>();
    builder.Services.AddScoped<IValveOperationRepository, InMemoryValveOperationRepository>();
    builder.Services.AddScoped<IConsumptionReadingRepository, InMemoryConsumptionReadingRepository>();
    builder.Services.AddScoped<IThresholdRepository, InMemoryThresholdRepository>();
    builder.Services.AddScoped<IAnomalyRepository, InMemoryAnomalyRepository>();
    builder.Services.AddScoped<IAlertRepository, InMemoryAlertRepository>();
    builder.Services.AddScoped<IIncidentRepository, InMemoryIncidentRepository>();
    builder.Services.AddScoped<INotificationChannelRepository, InMemoryNotificationChannelRepository>();
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not configured.");

    builder.Services.AddSingleton<AuditableEntityInterceptor>();
    builder.Services.AddDbContext<EfAppDbContext>((serviceProvider, options) =>
    {
        options.UseNpgsql(connectionString);
        options.AddInterceptors(serviceProvider.GetRequiredService<AuditableEntityInterceptor>());
    });

    builder.Services.AddScoped<DatabaseSeeder>();
    builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
    builder.Services.AddScoped<ISiteRepository, EfSiteRepository>();
    builder.Services.AddScoped<IRoomRepository, EfRoomRepository>();
    builder.Services.AddScoped<IDeviceGroupRepository, EfDeviceGroupRepository>();
    builder.Services.AddScoped<IDeviceRepository, EfDeviceRepository>();
    builder.Services.AddScoped<ISensorRepository, EfSensorRepository>();
    builder.Services.AddScoped<IValveRepository, EfValveRepository>();
    builder.Services.AddScoped<IDeviceCommandRepository, EfDeviceCommandRepository>();
    builder.Services.AddScoped<IValveOperationRepository, EfValveOperationRepository>();
    builder.Services.AddScoped<IConsumptionReadingRepository, EfConsumptionReadingRepository>();
    builder.Services.AddScoped<IThresholdRepository, EfThresholdRepository>();
    builder.Services.AddScoped<IAnomalyRepository, EfAnomalyRepository>();
    builder.Services.AddScoped<IAlertRepository, EfAlertRepository>();
    builder.Services.AddScoped<IIncidentRepository, EfIncidentRepository>();
    builder.Services.AddScoped<INotificationChannelRepository, EfNotificationChannelRepository>();
}

builder.Services.AddScoped<ISiteCommandService, SiteCommandService>();
builder.Services.AddScoped<ISiteQueryService, SiteQueryService>();
builder.Services.AddScoped<IDeviceControlContextFacade, DeviceControlContextFacade>();
builder.Services.AddScoped<IDeviceCommandService, DeviceCommandService>();
builder.Services.AddScoped<IValveOperationCommandService, ValveOperationCommandService>();
builder.Services.AddScoped<IDeviceCommandQueryService, DeviceCommandQueryService>();
builder.Services.AddScoped<IValveOperationQueryService, ValveOperationQueryService>();
builder.Services.AddScoped<IDeviceControlMitigationSummaryQueryService, DeviceControlMitigationSummaryQueryService>();
builder.Services.AddScoped<AnomalyDetectionService>();
builder.Services.AddScoped<IReadingCommandService, ReadingCommandService>();
builder.Services.AddScoped<IThresholdCommandService, ThresholdCommandService>();
builder.Services.AddScoped<IAnomalyCommandService, AnomalyCommandService>();
builder.Services.AddScoped<IReadingQueryService, ReadingQueryService>();
builder.Services.AddScoped<IThresholdQueryService, ThresholdQueryService>();
builder.Services.AddScoped<IAnomalyQueryService, AnomalyQueryService>();
builder.Services.AddScoped<IMonitoringSummaryQueryService, MonitoringSummaryQueryService>();
builder.Services.AddScoped<INotificationContextFacade, NotificationContextFacade>();
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();
builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();
builder.Services.AddScoped<IIncidentCommandService, IncidentCommandService>();
builder.Services.AddScoped<IIncidentMitigationCommandService, IncidentMitigationCommandService>();
builder.Services.AddScoped<IIncidentQueryService, IncidentQueryService>();
builder.Services.AddScoped<INotificationChannelCommandService, NotificationChannelCommandService>();
builder.Services.AddScoped<INotificationChannelQueryService, NotificationChannelQueryService>();
builder.Services.AddScoped<INotificationSummaryQueryService, NotificationSummaryQueryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapLightweightSwaggerUi();
}

if (!useInMemory && app.Configuration.GetValue<bool>("Persistence:SeedOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();
app.UseCors(frontendCorsPolicy);
app.UseAuthorization();
app.MapControllers();
app.Run();
