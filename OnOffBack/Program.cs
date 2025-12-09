using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Core.Services;
using FluentValidation;
using Infraestructure.Filters;
using Infraestructure.Repositories;
using Infraestructure.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
});

#region Configuration JWT
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.FromDays(1)
    };
});
#endregion

#region Confoguracion de validaciones 
builder.Services.AddScoped<IValidator<TaskUser>, TaskUserValidator>();
#endregion

#region Configuracion de inyeccion de dependencias 
builder.Services.AddScoped<IAdminInterfaces, AdminInterfaces>();

builder.Services.AddTransient<IBaseService<Users>, UsersService>();
builder.Services.AddTransient<IUsersService, UsersService>();

builder.Services.AddTransient<IBaseService<TaskUser>, TaskUserService>();
builder.Services.AddTransient<ITaskUserService, TaskUserService>();

builder.Services.AddTransient<IValidarEmailService, ValidarEmailService>();

builder.Services.AddTransient<FilterExceptions>();
#endregion

try
{
    string cadenaConnection = builder.Configuration.GetConnectionString("SqlServerConnection");
    builder.Services.AddDbContext<BaseDeDatosContext>(options =>
         options.UseSqlServer(cadenaConnection)
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information));
}
catch (Exception e)
{
    EntityBaseException entityBaseException = new EntityBaseException();
    entityBaseException.Message = $"Connection Data Base Error : {e.Message}";
    entityBaseException.Name = "Internal Server Error";

    InternalServerErrorBusinessExceprions ex = new InternalServerErrorBusinessExceprions(entityBaseException);
    throw ex;
}
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FilterExceptions));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
