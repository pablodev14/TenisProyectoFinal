using MyRealAPI.Repositories;
using MyRealAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Una sola instancia mantiene el catálogo cargado mientras funciona la API.
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Carga y valida el CSV al iniciar.
app.Services.GetRequiredService<IProductRepository>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "MyRealAPI - Products"));
}

app.MapControllers();
app.Run();
