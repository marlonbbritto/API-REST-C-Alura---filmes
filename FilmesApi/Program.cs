using FilmesApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("FilmeConnection");

builder.Services.AddDbContext<FilmeContext>(opts=> opts.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

// Adicione serviços ao contêiner
builder.Services.AddControllers();

// Adicione o redirecionamento HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5001;
});

var app = builder.Build();

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Middleware de redirecionamento HTTPS
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
