using System.Text.Json.Serialization;
using api.Context;
using api.Repositories;
using api.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Menambahkan context dari database
builder.Services.AddDbContext<MyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("APIContext")));
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<AccountRepository>();

builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfiguration"));

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetService<IConfiguration>();

builder.Services.AddCors(c => {
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});

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

app.UseCors(options => options.AllowAnyOrigin());

app.Run();
