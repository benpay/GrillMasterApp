using GrillMaster.Application.UseCases;
using GrillMaster.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var menuApiBaseUrl = builder.Configuration["ExternalApi:MenuApiBaseUrl"]
    ?? throw new InvalidOperationException("MenuApiBaseUrl is not configured.");

builder.Services.AddControllers();
builder.Services.AddInfrastructure(menuApiBaseUrl);
builder.Services.AddScoped<PlanGrillSessionsUseCase>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "GrillMaster API",
        Version = "v1",
        Description = "Optimizes barbecue item placement across grill sessions."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GrillMaster v1"));
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();