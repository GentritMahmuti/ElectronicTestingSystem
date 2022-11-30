using AutoMapper;
using ElectronicTestingSystem.Data;
using ElectronicTestingSystem.Data.UnitOfWork;
using ElectronicTestingSystem.Helper;
using ElectronicTestingSystem.Helpers;
using ElectronicTestingSystem.Services;
using ElectronicTestingSystem.Services.IService;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mapperConfiguration = new MapperConfiguration(
    mc => mc.AddProfile(new AutoMapperConfigurations()));

IMapper mapper = mapperConfiguration.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddDbContext<ElectronicTestingSystemDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<IExamService, ExamService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


var smtpConfigurations = builder.Configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
builder.Services.AddSingleton(smtpConfigurations);
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
