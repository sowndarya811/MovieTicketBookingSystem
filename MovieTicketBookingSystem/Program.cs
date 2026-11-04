
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Middleware;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Services.Interfaces;
using MovieTicketBookingSystem.Authorization;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CORS CONFIGURATION
// ==========================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ==========================================
// 2. CONTROLLERS
// ==========================================

builder.Services.AddControllers();

// ==========================================
// 3. JWT AUTHENTICATION
// ==========================================

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    )
            };
    });

// ==========================================
// 4. AUTHORIZATION
// ==========================================

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthorizationHandler,
    BookingAuthorizationHandler>();

// ==========================================
// 5. DATABASE CONFIGURATION
// ==========================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")
    ));

// ==========================================
// 6. DEPENDENCY INJECTION
// ==========================================

// Movie
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

// Theatre
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();

// Screen
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<IScreenService, ScreenService>();

// Seat
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

// Show
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

// Booking
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

// User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// JWT Service
builder.Services.AddScoped<IJwtService, JwtService>();

// ==========================================
// 7. OPEN API
// ==========================================

builder.Services.AddOpenApi();

// ==========================================
// 8. BUILD APPLICATION
// ==========================================

var app = builder.Build();

// ==========================================
// 9. ADMIN ACCOUNT INITIALIZATION
// ==========================================

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    var adminEmail =
        builder.Configuration["AdminSeed:Email"];

    var adminPassword =
        builder.Configuration["AdminSeed:Password"];

    if (!string.IsNullOrWhiteSpace(adminEmail) &&
        !string.IsNullOrWhiteSpace(adminPassword))
    {
        var adminExists =
            await context.Users
                .AnyAsync(u => u.Email == adminEmail);

        if (!adminExists)
        {
            var admin = new User
            {
                UserName = "Admin",
                Email = adminEmail,
                Role = "Admin"
            };

            var passwordHasher =
                new PasswordHasher<User>();

            admin.PasswordHash =
                passwordHasher.HashPassword(
                    admin,
                    adminPassword
                );

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}

// ==========================================
// 10. OPEN API IN DEVELOPMENT
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ==========================================
// 11. MIDDLEWARE PIPELINE
// ==========================================

// Global Exception Handler
app.UseMiddleware<GlobalExceptionHandler>();

// HTTPS Redirection
app.UseHttpsRedirection();

// Routing
app.UseRouting();

// Endpoint Metadata Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<EndpointMetadataMiddleware>();
}

// CORS
app.UseCors("AllowAngular");

// Authentication
app.UseAuthentication();

// Authentication Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthenticationDiagnosticsMiddleware>();

    app.UseMiddleware<AuthorizationDiagnosticsMiddleware>();
}

// Authorization
app.UseAuthorization();

// ==========================================
// 12. MAP CONTROLLERS
// ==========================================

app.MapControllers();

// ==========================================
// 13. RUN APPLICATION
// ==========================================

app.Run();
