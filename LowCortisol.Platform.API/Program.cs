using LowCortisol.Platform.API.DeviceControl.Application.Acl;
using LowCortisol.Platform.API.DeviceControl.Application.CommandServices;
using LowCortisol.Platform.API.DeviceControl.Application.Internal.Acl;
using LowCortisol.Platform.API.DeviceControl.Application.Internal.CommandServices;
using LowCortisol.Platform.API.DeviceControl.Application.Internal.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Application.QueryServices;
using LowCortisol.Platform.API.DeviceControl.Domain.Repositories;
using LowCortisol.Platform.API.Iam.Application.CommandServices;
using LowCortisol.Platform.API.Iam.Application.Internal.CommandServices;
using LowCortisol.Platform.API.Iam.Application.Internal.QueryServices;
using LowCortisol.Platform.API.Iam.Application.OutboundServices;
using LowCortisol.Platform.API.Iam.Application.QueryServices;
using LowCortisol.Platform.API.Iam.Domain.Repositories;
using LowCortisol.Platform.API.Iam.Infrastructure.Hashing.BCrypt;
using LowCortisol.Platform.API.Iam.Infrastructure.Tokens.Jwt;
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
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Seed;
using LowCortisol.Platform.API.Shared.Interfaces.Rest.OpenApi;
using LowCortisol.Platform.API.Workplace.Application.CommandServices;
using LowCortisol.Platform.API.Workplace.Application.Internal.CommandServices;
using LowCortisol.Platform.API.Workplace.Application.Internal.QueryServices;
using LowCortisol.Platform.API.Workplace.Application.QueryServices;
using LowCortisol.Platform.API.Workplace.Domain.Repositories;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Globalization;

using EfAppDbContext = LowCortisol.Platform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.AppDbContext;
using EfUserRepository = LowCortisol.Platform.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories.UserRepository;
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
using InMemoryUserRepository = LowCortisol.Platform.API.Iam.Infrastructure.Persistence.InMemory.Repositories.UserRepository;
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

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddLocalization();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SupportNonNullableReferenceTypes();
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "LowCortisol Platform API",
            Version = "v1",
            Description = "REST API for LowCortisol workplace, device control, monitoring and notification operations."
        });
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
    options.OperationFilter<LowCortisolOperationFilter>();
});

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
string? connectionString = null;

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
    builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();
}
else
{
    connectionString = PostgreSqlConnectionStringFactory.Create(builder.Configuration);

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
    builder.Services.AddScoped<IUserRepository, EfUserRepository>();
}

builder.Services.AddScoped<IPasswordHashingService, BCryptPasswordHashingService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
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

var supportedCultures = new[] { "es", "en", "pt" };
var requestLocalizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("es")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseForwardedHeaders();
app.UseExceptionHandler();
app.UseRequestLocalization(requestLocalizationOptions);
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "LowCortisol Platform API v1");
    options.RoutePrefix = "swagger";
});

if (!useInMemory && app.Configuration.GetValue<bool>("Persistence:CreateDatabaseOnStartup"))
{
    await PostgreSqlDatabaseInitializer.EnsureDatabaseCreatedAsync(connectionString!, app.Logger);
}

if (!useInMemory && app.Configuration.GetValue<bool>("Persistence:SeedOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<EfAppDbContext>();
    await context.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

app.UseHttpsRedirection();
app.UseCors(frontendCorsPolicy);
app.UseAuthorization();
app.MapControllers();
app.Run();
