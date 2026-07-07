using Cortex.Module.Auth.Infrastructure.Extensions;
using Cortex.Module.Issues.Infrastructure.Extensions;
using Scalar.AspNetCore;
using Cortex.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddIssuesInfrastructure(builder.Configuration);
builder.Services.AddAuthInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddCortexOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapScalarApiReference();



app.Run();


