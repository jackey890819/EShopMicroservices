var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddCarter();
builder.Services.AddCarter(null, config =>
{
    var modules = typeof(Program).Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();
    config.WithModules(modules);
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter(); // 自動Map所有實作了ICarterModule的class

app.Run();
