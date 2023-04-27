using ContactManagerApi.Models;
using ContactManagerApi.Services;
using ContactManagerApi.Utils;
using ContactManagerApi.Utils.Exceptions;
using ContactManagerApi.Utils.Policies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UserMangerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserMangerBDContext"));
});

builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LoginService>();

//Servicio para la autorizacion mediante token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//Servicio para las politicas de seguridad mediante claim
builder.Services.AddAuthorization(options => 
{
    options.AddPolicy("AdministradorCubano", policy => policy.Requirements.Add(new RolAdministradorCubanoRequierment()));
});
builder.Services.AddSingleton<IAuthorizationHandler, RolAdministradorCubanoHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//Exception handlers
app.ConfigureBuiltInExceptionHandler();

app.MapControllers();

app.Run();
