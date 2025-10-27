using Emata.Exercise.LoansManagement.Borrowers;
using Emata.Exercise.LoansManagement.Loans;
using Emata.Exercise.LoansManagement.Shared.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//add modules
builder.Services
    .AddBorrowersModule(builder.Configuration)
    .AddLoansModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapEndpoints();

//migrate module databases
await app.MigrateBorrowersDatabaseAsync();
await app.MigrateLoansDatabaseAsync();

app.Run();
