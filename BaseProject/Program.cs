using DTO;
using Marketplace;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middleware;
using Repositories;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Services.Hub;
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
builder.Services.Configure<JwtManagement>(builder.Configuration.GetSection("TokenManagement"));
var token = builder.Configuration.GetSection("TokenManagement").Get<JwtManagement>();

// ngambil email management dari appseting.json (option pattern)
builder.Services.Configure<EmailManagement>(builder.Configuration.GetSection("EmailManagement"));
var email = builder.Configuration.GetSection("EmailManagement").Get<EmailManagement>();

// Add signalR
builder.Services.AddSignalR();

// Add Pages
builder.Services.AddRazorPages();

// Add ConfigureLogs
ConfigureLogs();
builder.Host.UseSerilog();

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

// jika UseAuthentication dibawah UseAuthorization
// maka meskipun udah login, ketika hit api yang authorize
// dia bakalan return 401 (unauthorized)

// jika UseAuthentication di comment
// maka meskipun udah login, ketika hit api yang authorize
// namun bakalan return 401 (unauthorized)

// Middleware otentikasi JWT
app.UseAuthentication();

// jika UseAuthorization di comment
// maka meskipun udah login, tetap bisa hit api yang authorize
// namun bakalan error

// Middleware autorisasi
app.UseAuthorization();

app.UseMiddleware<BaseMiddleware>();

app.UseRouting();

app.UseStaticFiles();

// Map endpoint 
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/hubs/notification");
    endpoints.MapHub<UserHub>("/hubs/userCount");
    endpoints.MapRazorPages();
});

app.Run();


#region helper

void ConfigureLogs()
{
    // Get the environtment which the application is running on
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONTMENT");


    // Get the configuration
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();


    // Create Logger
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails() // Add detail Exception
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSearch(configuration))
        .CreateLogger();
}


ElasticsearchSinkOptions ConfigureElasticSearch(IConfigurationRoot configuration)
{
    var uri = new Uri(configuration["ElasticSearchConfiguration:Url"]);
    return new ElasticsearchSinkOptions(uri)
    {
        AutoRegisterTemplate = true,
        IndexFormat = "BaseProject" // Format indeks yang lebih baik menggunakan huruf kecil
    };
}

#endregion