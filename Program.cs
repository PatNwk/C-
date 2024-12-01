using Microsoft.EntityFrameworkCore;
using MonWebApp.Data;
using MonWebApp.Services;
using MonWebApp.Helpers;


var builder = WebApplication.CreateBuilder(args);

// Ajouter les services nécessaires à l'application
builder.Services.AddControllersWithViews();

// Configurer le contexte de base de données
builder.Services.AddDbContext<WalletContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
);

// Ajouter les services de session
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

// Ajouter le client HTTP pour Alpha Vantage
builder.Services.AddHttpClient("AlphaVantage", client =>
{
    client.BaseAddress = new Uri("https://www.alphavantage.co/");
});

// Ajouter le service StockService
builder.Services.AddTransient<StockService>();

// Ajouter le service TransactionService
builder.Services.AddTransient<TransactionService>();

// Configuration de la journalisation (logging)
builder.Logging.AddConsole();

var app = builder.Build();

// Configurer le routage et les middlewares
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Afficher les erreurs dans le navigateur
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Activer la gestion des sessions
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
