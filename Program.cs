using Muuki.Services;
using Muuki.Data;
using Muuki.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;
using Muuki.Services.Interfaces;
using MuukiAPI.Middleware;
using Muuki.Seed;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") 
    ?? throw new Exception("Falta la variable JWT_SECRET");
var mongoConnection = Environment.GetEnvironmentVariable("MONGO_CONNECTION");
var mongoDatabase = Environment.GetEnvironmentVariable("MONGO_DATABASE");

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton<JwtUtils>();
builder.Services.AddScoped<SpaceService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IBreedService, BreedService>();
builder.Services.AddScoped<ConditionSettingsService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var mongoContext = app.Services.GetRequiredService<MongoContext>();
var seeder = new ConditionSettingsSeeder(mongoContext);
await seeder.SeedAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseCors("AllowAllOrigins");

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();