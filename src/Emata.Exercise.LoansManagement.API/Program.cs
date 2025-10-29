using System.Reflection;
using Emata.Exercise.LoansManagement.Borrowers;
using Emata.Exercise.LoansManagement.Loans;
using Emata.Exercise.LoansManagement.Shared.Endpoints;
 
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Enable DI validation (important for testing)
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;      // catches scoped->singleton injection issues
    options.ValidateOnBuild = true;     // validates registrations when host builds
});

//We are using a modular architecture where each module is responsible for its own services registration
//Read more about modular monoliths here: 
// - https://www.milanjovanovic.tech/blog/what-is-a-modular-monolith
// - https://abp.io/architecture/modular-monolith
// - https://dev.to/xoubaman/modular-monolith-3fg1
builder.Services
    .AddBorrowersModule(builder.Configuration, [])
    .AddLoansModule(builder.Configuration, []);

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
