global using EmailSignUp_PasswordManagement.Models;
global using EmailSignUp_PasswordManagement.Data;
global using Microsoft.EntityFrameworkCore;
using EmailSignUp_PasswordManagement.Repositories;
using EmailSignUp_PasswordManagement.Repositories.Contracts;
using EmailSignUp_PasswordManagement.Services.Contracts;
using EmailSignUp_PasswordManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
