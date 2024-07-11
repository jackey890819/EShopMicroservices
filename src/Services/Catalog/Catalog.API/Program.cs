using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Extentions;
using Catalog.API.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddCarter();
//builder.Services.AddCarter(null, config =>
//{
//    var modules = typeof(Program).Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();
//    config.WithModules(modules);
//});

var programAssembly = typeof(Program).Assembly;

builder.Services.AddCarterWithAssemblies(programAssembly);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(programAssembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));  // ����
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));     // �O��
});

builder.Services.AddValidatorsFromAssembly(programAssembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
// �}�o���Ҫ��t�m
if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();    // �ˬd�ô��J�w�]Product
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter(); // �۰�Map�Ҧ���@�FICarterModule��class

//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    exceptionHandlerApp.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exception == null) return;

//        var problemDetails = new ProblemDetails
//        {
//            Title = exception.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exception.StackTrace
//        };
//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);

//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";

//        await context.Response.WriteAsJsonAsync(problemDetails);
//    });
//});
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
