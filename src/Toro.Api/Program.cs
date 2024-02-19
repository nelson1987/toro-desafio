using FluentValidation;
using System.Text.Json.Serialization;
using Toro.Api.Controllers;
using Toro.Core.Features.Transfers.Deposit;
using Toro.Core.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<CreateMovementCommand>, CreateMovementCommandValidator>()
                .AddScoped<ICreateMovementHandler, DepositHandler>()
                .AddScoped<IAccountRepository, AccountRepository>()
                .AddScoped<ITrendRepository,TrendRepository>();

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