using Clients.API.Extentions;
using Clients.Application;
using Flow.IdentityService;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var configuration = builder.Configuration;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();
// Enabled authentication
builder.Services.AddJwtBearerConfig(configuration);

// Configure Swagger
builder.Services.ConfigureSecuredSwagger();

// Configure Services
builder.Services.AddApplication(configuration);
builder.Services.AddIdentityService(configuration);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // Enable the "Authorize" button in the Swagger UI
        ui.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
