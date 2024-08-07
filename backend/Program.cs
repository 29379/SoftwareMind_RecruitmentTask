using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Interfaces.Repositories;
using HotDeskBookingSystem.Interfaces.ServiceInterfaces;
using HotDeskBookingSystem.Interfaces.Services;
using HotDeskBookingSystem.Repositories;
using HotDeskBookingSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        /*options.Authority = "https://dev-7z1v7v7v.eu.auth0.com/";
        options.Audience = "https://localhost:5001";*/
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("EmployeePolicy", policy => policy.RequireRole("EMPLOYEE", "ADMIN"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingStatusRepository, BookingStatusRepository>();
builder.Services.AddScoped<IDeskRepository, DeskRepository>();
builder.Services.AddScoped<IOfficeFloorRepository, OfficeFloorRepository>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IInputVerificationService, InputVerificationService>();


builder.Services.AddHostedService<ClearBookingsService>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ContractResolver = new DefaultContractResolver());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnyOrigin");
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
