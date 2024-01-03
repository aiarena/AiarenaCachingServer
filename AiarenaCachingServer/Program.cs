using AiArenaCachingServer.Controllers;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Configuration
    .AddJsonFile("config/appsettings.json");
    
builder.Services.AddSingleton<CachingSingleton>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();

