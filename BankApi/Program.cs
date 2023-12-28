using BankApi.Application.BankAccounts.Commands;
using BankApi.Application.BankAccounts.Validators;
using BankApi.Application.Common;
using BankApi.Application.Common.Behaviours;
using BankApi.Application.Common.Interfaces;
using BankApi.Application.Identity.Service;
using BankApi.Infrastructure.Configuration;
using BankApi.Infrastructure.Data;
using BankApi.Infrastructure.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value),
    ServiceLifetime.Scoped
);

builder.Services.Configure<CryptoSettings>(builder.Configuration.GetSection("CryptoSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<ICryptoService, CryptoService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UnitOfWork).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<IValidator<TransferMoneyCommand>, TransferMoneyValidator>();
builder.Services.AddTransient<IValidator<DepositMoneyCommand>, DepositMoneyValidator>();
builder.Services.AddTransient<IValidator<WithdrawMoneyCommand>, WithdrawMoneyValidator>();


builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
 options.TokenValidationParameters = new TokenValidationParameters
 {
     ValidateIssuer = false,
     ValidateAudience = false,
     ValidateLifetime = true,
     ValidateIssuerSigningKey = true,
     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt").GetSection("Key").Value))
     };
 });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
