using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using RoomReservationApiNet.Data;
using RoomReservationApiNet.Middleware;
using RoomReservationApiNet.Services;
using RoomReservationApiNet.Repository;
using System.Text;
using RoomReservationApiNet.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

// =========================
// JWT CONFIG
// =========================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret key is not configured.");
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// =========================
// DATABASE CONFIG
// =========================
var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? throw new InvalidOperationException("Database settings are not configured.");

switch (databaseSettings.Provider)
{
    case "SqlServer":
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(databaseSettings.ConnectionStrings.SqlServer));
        break;
    case "MySQL":
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(databaseSettings.ConnectionStrings.MySQL, ServerVersion.AutoDetect(databaseSettings.ConnectionStrings.MySQL)));
        break;
    case "PostgreSQL":
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(databaseSettings.ConnectionStrings.PostgreSQL));
        break;
    case "SQLite":
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(databaseSettings.ConnectionStrings.SQLite));
        break;
    case "MongoDB":
        // MongoDB-specific configuration
        break;
    default:
        throw new ArgumentException("Unsupported database provider");
}

// =========================
// SERVICES
// =========================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddHttpClient<EmailService>();
builder.Services.AddScoped<IReservationService>(provider =>
   new ReservationService(
       provider.GetRequiredService<IReservationRepository>(),
       provider.GetRequiredService<IUserRepository>(),
       provider.GetRequiredService<IHttpContextAccessor>(),
       provider.GetRequiredService<EmailService>(),
       provider.GetRequiredService<ILogger<ReservationService>>(),
       provider.GetRequiredService<IRoomRepository>(),
       provider.GetRequiredService<IReservationStatusRepository>()
   )
);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IReservationStatusService, ReservationStatusService>();
builder.Services.AddScoped<IReservationStatusRepository, ReservationStatusRepository>();
builder.Services.AddScoped<IEmailQueueService, EmailQueueService>();
builder.Services.AddHttpContextAccessor();

// =========================
// EMAIL CONFIG
// =========================
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddScoped<EmailConfiguration>(provider =>
    provider.GetRequiredService<IOptions<EmailConfiguration>>().Value);

// =========================
// STRIPE CONFIG
// =========================
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<IStripeService, StripeService>();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "RoomReservationApiNet", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// =========================
// PIPELINE
// =========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
