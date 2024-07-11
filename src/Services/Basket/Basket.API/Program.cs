var builder = WebApplication.CreateBuilder(args);
// add service to the container.

var app = builder.Build();
// configure the http request pipeline

app.Run();
