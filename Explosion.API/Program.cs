using System.Text;
using System.Text.Json;
using Explosion.API.Data;
using Explosion.API.Repositories;
using Explosion.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
var jwtKeyFromEnvironment = Environment.GetEnvironmentVariable("JWT__KEY")
    ?? Environment.GetEnvironmentVariable("JWT_KEY");
var jwtKeyFromConfig = builder.Configuration["Jwt:Key"];
var jwtKey = !string.IsNullOrWhiteSpace(jwtKeyFromEnvironment)
    ? jwtKeyFromEnvironment
    : builder.Environment.IsDevelopment()
        ? jwtKeyFromConfig
        : null;

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT key not configured. Set environment variable JWT__KEY (or JWT_KEY).");
}

if (jwtKey.Length < 32)
{
    throw new InvalidOperationException(
        "JWT key must have at least 32 characters.");
}

builder.Configuration["Jwt:Key"] = jwtKey;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<ExpDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ProductRep>();
builder.Services.AddScoped<UserRep>();

builder.Services.AddScoped<ProductServ>();
builder.Services.AddScoped<UserServ>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IPasswordHasher<Explosion.API.Models.User>, PasswordHasher<Explosion.API.Models.User>>();
builder.Services.AddScoped<CartRep>();
builder.Services.AddScoped<FavoriteRep>();

builder.Services.AddScoped<CartServ>();
builder.Services.AddScoped<FavoriteServ>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
        // .AllowCredentials(); // enable only if you use cookies/sessions
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireSignedTokens = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    const string schemeId = "Bearer";

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Explosion API",
        Version = "v1"
    });

    options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "Cole apenas o token JWT"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(schemeId, document)] = []
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    await next();
});

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
