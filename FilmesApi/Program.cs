using FilmesApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configuração da conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("FilmeConnection");
builder.Services.AddDbContext<FilmeContext>(opts => opts.UseLazyLoadingProxies().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configuração do AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Adicionar serviços ao contêiner
builder.Services.AddControllers().AddNewtonsoftJson();

// Adicionar Swagger com Swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FilmesAPI", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Adicionar redirecionamento HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5001;
});

var app = builder.Build();

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Habilitar middleware do Swagger apenas no desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Filmes API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
