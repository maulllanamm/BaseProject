using DTO;
using Marketplace;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middleware;
using Repositories;
using Services;
using Services.Interface;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);

// dependencies
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddAutoMapper(typeof(AutoMapConfig));

// ngambil token management dari appseting.json (option pattern)
builder.Services.Configure<JwtDTO>(builder.Configuration.GetSection("TokenManagement"));
var token = builder.Configuration.GetSection("TokenManagement").Get<JwtDTO>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// instal package Microsoft.AspNetCore.Authentication.Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Secret)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = token.Issuer,
            ValidAudience = token.Audience,
        };
    });

var app = builder.Build();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
context.Database.EnsureCreated();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware otentikasi JWT
app.UseAuthentication();

// Middleware autorisasi
app.UseAuthorization();

app.UseMiddleware<BaseMiddleware>();

// Map endpoint ke controller
app.MapControllers();

app.Run();
